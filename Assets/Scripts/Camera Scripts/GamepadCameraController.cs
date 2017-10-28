using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class GamepadCameraController : MonoBehaviour
{
    [SerializeField, Range(10f, 100f)] float firstPersonCameraSpeed = 60f;
    [SerializeField, Range(40f, 130f)] float thirdPersonCameraSpeed = 90f;

    Transform playerTransform;
    Rigidbody playerRB;
    Transform cameraContainerTransform;
    ThirdPersonCamera thirdPersonCamera;
    Vector3 thirdPersonTranslation;

    float timeTillStartReaglign = 2f;
    float timeSinceLastCameraMove = 0f;
    bool realigning = false;

    void Start()
    {
        cameraContainerTransform = GetComponent<Transform>();
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        thirdPersonCamera = GameManager.player.GetComponentInChildren<ThirdPersonCamera>();
        playerTransform = GameManager.player.GetComponent<Transform>();

        thirdPersonTranslation = thirdPersonCamera.ThirdPersonAnchor.position - thirdPersonCamera.FirstPersonAnchor.position;

        if (!VRDevice.isPresent)
            StartCoroutine(CameraControllerCoroutine());
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //reset our camera rotation to the player's rotation whenever we change scenes
        if (!VRDevice.isPresent)
            cameraContainerTransform.eulerAngles = playerRB.transform.eulerAngles;
    }

    void ThirdPersonCameraMove()
    {
        ThirdPersonCameraAlign();

        float cameraPitch, cameraYaw;
#if DEBUGGER
        if (BuildDebugger.WASD)
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVerticalWASD") * thirdPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontalWASD") * thirdPersonCameraSpeed * Time.deltaTime;
        }
        else
#endif
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVertical") * thirdPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * thirdPersonCameraSpeed * Time.deltaTime;
        }

        //make the pitch not go above 80 degrees, and not go below 335 degrees
        cameraPitch = Quaternion.Euler(new Vector3(cameraPitch, 0f, 0f)).eulerAngles.x;

        //camera position is lower than the player
        if (cameraPitch > 180)
        {
            if (cameraPitch < 335f)
                cameraPitch = 335f;
        }
        //camera position is higher than the player
        else
        {
            if (cameraPitch > 80f)
                cameraPitch = 80f;
        }

        cameraContainerTransform.rotation = Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0f));

        cameraContainerTransform.position = thirdPersonCamera.FirstPersonAnchor.position;
        cameraContainerTransform.Translate(thirdPersonTranslation);
    }

    void FirstPersonCameraMove()
    {
        float cameraPitch, cameraYaw;
#if DEBUGGER
        if (BuildDebugger.WASD)
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVerticalWASD") * firstPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontalWASD") * firstPersonCameraSpeed * Time.deltaTime;
        }
        else
#endif
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVertical") * firstPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * firstPersonCameraSpeed * Time.deltaTime;
        }

        cameraContainerTransform.rotation = (Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0.0f)));
    }

    void ReAlignCamera(float alignRate)
    {
        Quaternion alignQuaternion = Quaternion.Euler(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, 0f);
        cameraContainerTransform.rotation = Quaternion.Slerp(cameraContainerTransform.rotation, alignQuaternion, Time.deltaTime * alignRate);
    }

    void ThirdPersonCameraAlign()
    {
        if (!realigning)
        {
#if DEBUGGER
            if ((BuildDebugger.WASD && Input.GetAxis("RVerticalWASD") == 0f && Input.GetAxis("RHorizontalWASD") == 0f) ||
            (!BuildDebugger.WASD && Input.GetAxis("RVertical") == 0f && Input.GetAxis("RHorizontal") == 0f))
#else
            if (Input.GetAxis("RVertical") == 0f && Input.GetAxis("RHorizontal") == 0f)
#endif
                timeSinceLastCameraMove += Time.deltaTime;
            else
                timeSinceLastCameraMove = 0f;

            //start realigning if the player hasn't moved in a while
            if (timeSinceLastCameraMove > timeTillStartReaglign)
                realigning = true;
        }
        else
        {
            //make sure the player isn't trying to move the camera about
#if DEBUGGER
            if ((BuildDebugger.WASD && Input.GetAxis("RVerticalWASD") == 0f && Input.GetAxis("RHorizontalWASD") == 0f) ||
            (!BuildDebugger.WASD && Input.GetAxis("RVertical") == 0f && Input.GetAxis("RHorizontal") == 0f))
#else
            if (Input.GetAxis("RVertical") == 0f && Input.GetAxis("RHorizontal") == 0f)
#endif
                ReAlignCamera(2f);
            else
            {
                realigning = false;
                timeSinceLastCameraMove = 0f;
            }
        }
    }

    //private void LateUpdate()
    //{
    //    if (!VRDevice.isPresent)
    //    {
    //        if (thirdPersonCamera.UpdatingCameraPosition)
    //            ReAlignCamera(2f);

    //        else if (thirdPersonCamera.UsingThirdPersonCamera)
    //            ThirdPersonCameraMove();

    //        else
    //            FirstPersonCameraMove();
    //    }      
    //}

    //private void Update()
    //{
    //    if (!VRDevice.isPresent)
    //    {
    //        if (thirdPersonCamera.UpdatingCameraPosition)
    //            ReAlignCamera(2f);

    //        else if (thirdPersonCamera.UsingThirdPersonCamera)
    //            ThirdPersonCameraMove();

    //        else
    //            FirstPersonCameraMove();
    //    }      
    //}

    IEnumerator CameraControllerCoroutine()
    {
        yield return null;

        //while updating to the third/first person camera, re-align the camera 
        if (thirdPersonCamera.UpdatingCameraPosition)
            ReAlignCamera(2f);

        else if (thirdPersonCamera.UsingThirdPersonCamera)
            ThirdPersonCameraMove();

        else
            FirstPersonCameraMove();

        StartCoroutine(CameraControllerCoroutine());
    }
}