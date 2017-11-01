using System.Collections;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;
public class GamepadCameraController : MonoBehaviour
{
    [SerializeField, Range(10.0f, 100.0f)] private float firstPersonCameraSpeed = 60.0f;
    [SerializeField, Range(40.0f, 130.0f)] private float thirdPersonCameraSpeed = 90.0f;
    private Transform playerTransform = null, cameraContainerTransform = null;
    private Rigidbody playerRB = null;
    private ThirdPersonCamera thirdPersonCamera = null;
    private Vector3 thirdPersonTranslation;
    private const float timeTillStartReaglign = 2.0f;
    private float timeSinceLastCameraMove = 0.0f;
    private bool realigning = false;
    private void Start()
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
        if (!VRDevice.isPresent)
            cameraContainerTransform.eulerAngles = playerRB.transform.eulerAngles;
    }
    private void ThirdPersonCameraMove()
    {
        ThirdPersonCameraAlign();
        float cameraPitch, cameraYaw;
#if DEBUGGER
        if (BuildDebugger.WASD)
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x - Input.GetAxis("RVerticalWASD") * thirdPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontalWASD") * thirdPersonCameraSpeed * Time.deltaTime;
        }
        else
#endif
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x - Input.GetAxis("RVertical") * thirdPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * thirdPersonCameraSpeed * Time.deltaTime;
        }
        cameraPitch = Quaternion.Euler(cameraPitch, 0.0f, 0.0f).eulerAngles.x;
        if (cameraPitch > 180.0f)
        {
            if (cameraPitch < 335.0f)
                cameraPitch = 335.0f;
        }
        else if (cameraPitch > 80.0f)
            cameraPitch = 80.0f;
        cameraContainerTransform.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0.0f);
        cameraContainerTransform.position = thirdPersonCamera.FirstPersonAnchor.position;
        cameraContainerTransform.Translate(thirdPersonTranslation);
    }
    private void FirstPersonCameraMove()
    {
        float cameraPitch, cameraYaw;
#if DEBUGGER
        if (BuildDebugger.WASD)
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x - Input.GetAxis("RVerticalWASD") * firstPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontalWASD") * firstPersonCameraSpeed * Time.deltaTime;
        }
        else
#endif
        {
            cameraPitch = cameraContainerTransform.eulerAngles.x - Input.GetAxis("RVertical") * firstPersonCameraSpeed * Time.deltaTime;
            cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * firstPersonCameraSpeed * Time.deltaTime;
        }
        cameraContainerTransform.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0.0f);
    }
    private void ReAlignCamera(float alignRate)
    {
        cameraContainerTransform.rotation = Quaternion.Slerp(cameraContainerTransform.rotation, Quaternion.Euler(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, 0.0f), Time.deltaTime * alignRate);
    }
    private void ThirdPersonCameraAlign()
    {
        if (!realigning)
        {
#if DEBUGGER
            if ((BuildDebugger.WASD && 0.0f == Input.GetAxis("RVerticalWASD") && 0.0f == Input.GetAxis("RHorizontalWASD")) ||
            (!BuildDebugger.WASD && 0.0f == Input.GetAxis("RVertical") && 0.0f == Input.GetAxis("RHorizontal")))
#else
            if (0.0f == Input.GetAxis("RVertical") && 0.0f == Input.GetAxis("RHorizontal"))
#endif
                timeSinceLastCameraMove += Time.deltaTime;
            else
                timeSinceLastCameraMove = 0.0f;
            if (timeSinceLastCameraMove > timeTillStartReaglign)
                realigning = true;
        }
        else
        {
#if DEBUGGER
            if ((BuildDebugger.WASD && 0.0f == Input.GetAxis("RVerticalWASD") && 0.0f == Input.GetAxis("RHorizontalWASD")) ||
            (!BuildDebugger.WASD && 0.0f == Input.GetAxis("RVertical") && 0.0f == Input.GetAxis("RHorizontal")))
#else
            if (0.0f == Input.GetAxis("RVertical") && 0.0f == Input.GetAxis("RHorizontal"))
#endif
                ReAlignCamera(2.0f);
            else
            {
                realigning = false;
                timeSinceLastCameraMove = 0.0f;
            }
        }
    }
    private IEnumerator CameraControllerCoroutine()
    {
        yield return null;
        if (thirdPersonCamera.UpdatingCameraPosition)
            ReAlignCamera(2.0f);
        else if (thirdPersonCamera.UsingThirdPersonCamera)
            ThirdPersonCameraMove();
        else
            FirstPersonCameraMove();
        StartCoroutine(CameraControllerCoroutine());
    }
}