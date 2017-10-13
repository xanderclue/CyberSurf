using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class GamepadCameraController : MonoBehaviour
{
    [SerializeField, Range(10f, 100f)] float firstPersonCameraSpeed = 60f;
    float thirdPersonCameraSpeed = 90f;

    Transform cameraContainerTransform;
    Transform playerTransform;
    ThirdPersonCamera thirdPersonCamera;
    Rigidbody playerRB;

    Vector3 thirdPersonTranslation;

    void Start ()
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
        //reset our camera position to the player's rotation, if we were using debug camera rotation
        if (!VRDevice.isPresent)
            cameraContainerTransform.eulerAngles = playerRB.transform.eulerAngles;
    }

    void ThirdPersonCameraMove()
    {
        cameraContainerTransform.position = thirdPersonCamera.FirstPersonAnchor.position;

        float cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVertical") * thirdPersonCameraSpeed * Time.deltaTime;
        float cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * thirdPersonCameraSpeed * Time.deltaTime;

        //make the pitch not go above 90, and not go below 335

        cameraPitch = Quaternion.Euler(new Vector3(cameraPitch, 0f, 0f)).eulerAngles.x;

        //camera position is lower than the player
        if (cameraPitch > 180)
        {
            if (cameraPitch < 335f)
                cameraPitch = 335f;
        }
        else
        {
            if (cameraPitch > 80f)
                cameraPitch = 80f;
        }

        print("Camera Pitch: " + cameraPitch);

        cameraContainerTransform.rotation = (Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0f)));

        cameraContainerTransform.Translate(thirdPersonTranslation);
    }

    void FirstPersonCameraMove()
    {
        float cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVertical") * firstPersonCameraSpeed * Time.deltaTime;
        float cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * firstPersonCameraSpeed * Time.deltaTime;

        cameraContainerTransform.rotation = (Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0.0f)));
    }

    void ReAlignCamera()
    {
        Quaternion alignQuaternion = Quaternion.Euler(0f, playerTransform.eulerAngles.y, 0f);
        cameraContainerTransform.rotation = Quaternion.Slerp(cameraContainerTransform.rotation, alignQuaternion, Time.deltaTime * 2f);
    }

    IEnumerator CameraControllerCoroutine()
    {
        yield return null;

        //while updating to the third person camera, re-align the camera 
        if (thirdPersonCamera.UpdatingCameraPosition)
            ReAlignCamera();

        else if (thirdPersonCamera.UsingThirdPersonCamera)
            ThirdPersonCameraMove();

        else
            FirstPersonCameraMove();

        StartCoroutine(CameraControllerCoroutine());
    }
}
