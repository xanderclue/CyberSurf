using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class KeyInputManager : MonoBehaviour
{
#if UNITY_STANDALONE_OSX
    public const KeyCode XBOX_A = KeyCode.JoystickButton16;
    public const KeyCode XBOX_X = KeyCode.JoystickButton18;
    public const KeyCode XBOX_Y = KeyCode.JoystickButton19;
    public const KeyCode XBOX_LB = KeyCode.JoystickButton13;
    public const KeyCode XBOX_RB = KeyCode.JoystickButton14;
    public const KeyCode XBOX_BACK = KeyCode.JoystickButton10;
    public const KeyCode XBOX_START = KeyCode.JoystickButton9;
#elif UNITY_STANDALONE_WIN
    public const KeyCode XBOX_A = KeyCode.JoystickButton0;
    public const KeyCode XBOX_X = KeyCode.JoystickButton2;
    public const KeyCode XBOX_Y = KeyCode.JoystickButton3;
    public const KeyCode XBOX_LB = KeyCode.JoystickButton4;
    public const KeyCode XBOX_RB = KeyCode.JoystickButton5;
    public const KeyCode XBOX_BACK = KeyCode.JoystickButton6;
    public const KeyCode XBOX_START = KeyCode.JoystickButton7;
#endif

    ManagerClasses.GameState state;

    //variables for returning back to menu
    public float flippedTimer = 3f;
    public bool hubOnFlippedHMD = false;
    bool countingDown = false;
    float timeUpsideDown = 0f;
    Quaternion flippedQuaternion;

    //calibration variables
    ThirdPersonCamera thirdPersonCameraScript;
    Transform playerTransform;
    Transform cameraContainer;
    Vector3 cameraContainerPositionDifference;

    public void setupKeyInputManager(ManagerClasses.GameState s)
    {
        #region TEXTURE2DARRAY
#if FALSE
        Texture2D[] arrayOfTextures, newArrayOfTextures;
        Texture2DArray textureArray;

        // initializing a new array of textures
        {
            int numTextures = 4;
            int texWid = 420;
            int texHei = 69;
            arrayOfTextures = new Texture2D[numTextures];
            for (int i = 0; i < numTextures; ++i)
                arrayOfTextures[i] = new Texture2D(texWid, texHei);
        }

        // copying textures from the array of textures to a new texture array
        {
            textureArray = new Texture2DArray(arrayOfTextures[0].width, arrayOfTextures[0].height, arrayOfTextures.Length, arrayOfTextures[0].format, false);
            for (int i = 0; i < arrayOfTextures.Length; ++i)
                Graphics.CopyTexture(arrayOfTextures[i], 0, textureArray, i);
        }

        // copying textures from the texture array to a new array of textures
        {
            newArrayOfTextures = new Texture2D[textureArray.depth];
            for (int i = 0; i < textureArray.depth; ++i)
                Graphics.CopyTexture(textureArray, i, newArrayOfTextures[i], 0);
        }
#endif
        #endregion

        state = s;
        thirdPersonCameraScript = GameManager.player.GetComponentInChildren<ThirdPersonCamera>();
        playerTransform = GameManager.player.GetComponent<Transform>();
        cameraContainer = GameManager.player.GetComponentInChildren<CameraCounterRotate>().GetComponent<Transform>();
        cameraContainerPositionDifference = cameraContainer.position - playerTransform.position;

        StartCoroutine(CalibrationCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state.currentState != GameStates.HubWorld)
            {
                GameManager.instance.lastPortalBuildIndex = -1;
                EventManager.OnTriggerTransition(1);
            }
            else
            {
                SaveLoader.save();
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(XBOX_BACK))
        {
            StartCoroutine(CalibrationCoroutine());
        }

        if (state.currentState != GameStates.HubWorld && Input.GetKeyDown(XBOX_START))
        {
            GameManager.instance.lastPortalBuildIndex = -1;
            EventManager.OnTriggerTransition(1);
        }

        //TODO:: don't let the player go into 3rd person in start or hub world
        if (Input.GetKeyDown(XBOX_Y))
        {
            thirdPersonCameraScript.UpdateThirdPersonCamera();
        }

        #region stuff we're going to get rid of
        if (state.currentState != GameStates.HubWorld && hubOnFlippedHMD && VRDevice.isPresent)
        {
            flippedQuaternion = InputTracking.GetLocalRotation(VRNode.Head);

            //if we're upside down, start the countdown and reset our timer
            if (flippedQuaternion.eulerAngles.z > 150f && flippedQuaternion.eulerAngles.z < 210f && !countingDown)
            {
                countingDown = true;
                timeUpsideDown = 0f;
            }
            else if (countingDown)
            {
                //if we're still upside down
                if (flippedQuaternion.eulerAngles.z > 150f && flippedQuaternion.eulerAngles.z < 210f)
                    timeUpsideDown += Time.deltaTime;
                else
                    countingDown = false;

                //go back to main menu once we've been upside down long enough
                if (timeUpsideDown > flippedTimer)
                {
                    countingDown = false;
                    EventManager.OnTriggerTransition(1);
                }
            }
        }
        #endregion
    }

    public IEnumerator CalibrationCoroutine()
    {
        //only calibrate if the camera isn't moving
        if (!thirdPersonCameraScript.UpdatingCameraPosition)
        {
            //wait for the end of the frame so we can catch positional data from the VR headset
            yield return new WaitForEndOfFrame();

            GameObject player = GameManager.player;

            Vector3 playerPosition = player.GetComponent<Transform>().position;
            Quaternion playerRotation = player.GetComponent<Transform>().rotation;

            //set the cameraContainer back on top of the board, in case we are re-calibrating
            cameraContainer.SetPositionAndRotation(playerPosition, playerRotation);
            cameraContainer.Translate(cameraContainerPositionDifference);

            Vector3 headPosition = player.GetComponentInChildren<ScreenFade>().transform.localPosition;
            Vector3 headRotation = player.GetComponentInChildren<ScreenFade>().transform.eulerAngles;

            //rotate, then translate

            //rotate the camera so that it is rotated in the same direction as the board
            float yRotation = Mathf.DeltaAngle(headRotation.y, cameraContainer.eulerAngles.y);
            cameraContainer.Rotate(Vector3.up * yRotation);

            //headPosition acts as though the cameraContainer is the ground
            //so if headPosition.y = 1.4, then the camera will be sitting 1.4 meters above the cameraContainer
            //therefore, translate the cameraContainer in opposite directions of wherever the headPosition is
            cameraContainer.Translate(headPosition * -1f);

            thirdPersonCameraScript.CalibrateThirdPersonAnchors(cameraContainer.position, playerRotation);
        }
    }

}
