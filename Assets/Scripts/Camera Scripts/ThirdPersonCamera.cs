using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] GameObject ACE;
    [SerializeField] Transform firstPersonAnchor;
    Transform thirdPersonAnchor;
    Transform cameraContainerTransform;
    float originalCameraContainerHeight;

    KeyInputManager keyInputManager;

    bool updatingThirdPersonCamera, usingThirdPersonCamera;

    public bool UpdatingThirdPersonCamera { get { return updatingThirdPersonCamera; } }
    public bool UsingThirdPersonCamera { get { return usingThirdPersonCamera; } }

    private void Start()
    {
        if (ACE.activeInHierarchy)
            ACE.SetActive(false);

        updatingThirdPersonCamera = usingThirdPersonCamera = false;

        thirdPersonAnchor = GetComponent<Transform>();
        cameraContainerTransform = GameManager.player.GetComponentInChildren<CameraCounterRotate>().transform;
        originalCameraContainerHeight = cameraContainerTransform.localPosition.y;
        keyInputManager = GameManager.instance.keyInputScript;       
    }

    public void UpdateThirdPersonCamera()
    {
        //only start our coroutine if we aren't already updating it
        if (!updatingThirdPersonCamera)
        {
            usingThirdPersonCamera = !usingThirdPersonCamera;
            StartCoroutine(MoveCameraContainer());
        }
    }

    IEnumerator MoveCameraContainer()
    {
        updatingThirdPersonCamera = true;

        //set our destination depending on if we are moving to or away from the third person camera
        if (usingThirdPersonCamera)
        {

        }

        //slowly move the camera into position
        //while()



        yield return null;

        updatingThirdPersonCamera = false;
    }
}
