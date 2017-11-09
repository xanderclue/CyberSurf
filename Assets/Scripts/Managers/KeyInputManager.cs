using System.Collections;
using UnityEngine;
using static KeyInputManager.VR;
public class KeyInputManager : MonoBehaviour
{
    public static class VR
    {
#if UNITY_2017_2_OR_NEWER
        public static bool VRPresent => UnityEngine.XR.XRDevice.isPresent;
        public static Quaternion GetHeadRotation() => UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head);
#else
        public static bool VRPresent => UnityEngine.VR.VRDevice.isPresent;
        public static Quaternion GetHeadRotation() => UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.Head);
#endif
    }
#if UNITY_STANDALONE_OSX
    public const KeyCode XBOX_A = KeyCode.JoystickButton16;
    public const KeyCode XBOX_X = KeyCode.JoystickButton18;
    public const KeyCode XBOX_Y = KeyCode.JoystickButton19;
    public const KeyCode XBOX_LB = KeyCode.JoystickButton13;
    public const KeyCode XBOX_BACK = KeyCode.JoystickButton10;
    public const KeyCode XBOX_START = KeyCode.JoystickButton9;
#else
    public const KeyCode XBOX_A = KeyCode.JoystickButton0;
    public const KeyCode XBOX_X = KeyCode.JoystickButton2;
    public const KeyCode XBOX_Y = KeyCode.JoystickButton3;
    public const KeyCode XBOX_LB = KeyCode.JoystickButton4;
    public const KeyCode XBOX_BACK = KeyCode.JoystickButton6;
    public const KeyCode XBOX_START = KeyCode.JoystickButton7;
#endif
    [SerializeField] private float flippedTimer = 3.0f;
    [SerializeField] private bool hubOnFlippedHMD = false;
    private ManagerClasses.GameState state = null;
    private bool countingDown = false;
    private float timeUpsideDown = 0.0f;
    private ThirdPersonCamera thirdPersonCameraScript = null;
    private Transform cameraContainer = null;
    private Quaternion flippedQuaternion;
    private Vector3 cameraContainerPositionDifference;
    public void SetupKeyInputManager(ManagerClasses.GameState s)
    {
        state = s;
        thirdPersonCameraScript = GameManager.player.GetComponentInChildren<ThirdPersonCamera>();
        cameraContainer = GameManager.player.GetComponentInChildren<CameraCounterRotate>().GetComponent<Transform>();
        cameraContainerPositionDifference = cameraContainer.position - GameManager.player.transform.position;
        StartCoroutine(CalibrationCoroutine());
    }
    private void Update()
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
                SaveLoader.Save();
                Application.Quit();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(XBOX_BACK))
            StartCoroutine(CalibrationCoroutine());
        if (state.currentState != GameStates.HubWorld && Input.GetKeyDown(XBOX_START))
        {
            GameManager.instance.lastPortalBuildIndex = -1;
            EventManager.OnTriggerTransition(1);
        }
        if (Input.GetKeyDown(XBOX_Y))
            thirdPersonCameraScript.UpdateThirdPersonCamera();
        if (VRPresent && hubOnFlippedHMD && state.currentState != GameStates.HubWorld)
        {
            flippedQuaternion = GetHeadRotation();
            if (flippedQuaternion.eulerAngles.z > 150.0f && flippedQuaternion.eulerAngles.z < 210.0f && !countingDown)
            {
                countingDown = true;
                timeUpsideDown = 0.0f;
            }
            else if (countingDown)
            {
                if (flippedQuaternion.eulerAngles.z > 150.0f && flippedQuaternion.eulerAngles.z < 210.0f)
                    timeUpsideDown += Time.deltaTime;
                else
                    countingDown = false;
                if (timeUpsideDown > flippedTimer)
                {
                    countingDown = false;
                    EventManager.OnTriggerTransition(1);
                }
            }
        }
    }
    private IEnumerator CalibrationCoroutine()
    {
        if (!thirdPersonCameraScript.UpdatingCameraPosition)
        {
            yield return new WaitForEndOfFrame();
            Transform player = GameManager.player.transform;
            Transform screenFade = player.GetComponentInChildren<ScreenFade>().transform;
            cameraContainer.SetPositionAndRotation(player.position, player.rotation);
            cameraContainer.Translate(cameraContainerPositionDifference);
            cameraContainer.Rotate(0.0f, Mathf.DeltaAngle(screenFade.eulerAngles.y, cameraContainer.eulerAngles.y), 0.0f);
            cameraContainer.Translate(-screenFade.localPosition);
            thirdPersonCameraScript.CalibrateThirdPersonAnchors(cameraContainer.position, player.rotation);
        }
    }
}