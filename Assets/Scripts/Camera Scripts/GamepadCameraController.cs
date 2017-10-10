using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class GamepadCameraController : MonoBehaviour
{
    [SerializeField, Range(10f, 100f)] float cameraSpeed = 60f;

    Transform cameraContainerTransform = null;
    Rigidbody playerRB;

    void Start ()
    {
        cameraContainerTransform = GetComponent<Transform>();
        playerRB = GameManager.player.GetComponent<Rigidbody>();

        if (!VRDevice.isPresent)
            StartCoroutine(CameraControllerCoroutine());
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //reset our camera position to the player's rotation, if we were using debug camera rotation
        if (!VRDevice.isPresent)
            cameraContainerTransform.eulerAngles = playerRB.transform.eulerAngles;
    }

    IEnumerator CameraControllerCoroutine()
    {
        yield return null;

        float cameraPitch = cameraContainerTransform.eulerAngles.x + -Input.GetAxis("RVertical") * cameraSpeed * Time.deltaTime;
        float cameraYaw = cameraContainerTransform.eulerAngles.y + Input.GetAxis("RHorizontal") * cameraSpeed * Time.deltaTime;

        cameraContainerTransform.rotation = (Quaternion.Euler(new Vector3(cameraPitch, cameraYaw, 0.0f)));

        StartCoroutine(CameraControllerCoroutine());
    }
}
