using System.Collections;
using UnityEngine;
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private GameObject ACE = null;
    [SerializeField] private EyeRayCaster eyeRayCaster = null;
    [SerializeField] private Transform firstPersonAnchor = null;
    [SerializeField] private Transform thirdPersonAnchor = null;
    [SerializeField] private Transform cameraContainerTransform = null;
    [SerializeField] private float translationRate = 2.0f;
    private const float aceFadeTime = 0.8f, tolerance = 0.05f;
    private float originalEyeRaycastLength = 0.0f, timeIntoFade = 0.0f, alpha = 0.0f;
    private Material aceMaterial = null;
    private bool updatingCameraPosition = false, usingThirdPersonCamera = false;
    public bool UpdatingCameraPosition { get { return updatingCameraPosition; } }
    public bool UsingThirdPersonCamera { get { return usingThirdPersonCamera; } }
    public Transform FirstPersonAnchor { get { return firstPersonAnchor; } }
    public Transform ThirdPersonAnchor { get { return thirdPersonAnchor; } }
    private void Start()
    {
        if (ACE.activeInHierarchy)
            ACE.SetActive(false);
        aceMaterial = ACE.GetComponent<MeshRenderer>().material;
        aceMaterial.color = new Color(aceMaterial.color.r, aceMaterial.color.g, aceMaterial.color.b, 0f);
        updatingCameraPosition = usingThirdPersonCamera = false;
        originalEyeRaycastLength = eyeRayCaster.rayCheckLength;
    }
    public void UpdateThirdPersonCamera()
    {
        if (!updatingCameraPosition)
            StartCoroutine(MoveCameraContainer());
    }
    public void CalibrateThirdPersonAnchors(Vector3 position, Quaternion rotation)
    {
        firstPersonAnchor.SetPositionAndRotation(position, rotation);
        if (usingThirdPersonCamera)
            cameraContainerTransform.position = thirdPersonAnchor.position;
        else
            cameraContainerTransform.position = firstPersonAnchor.position;
    }
    bool UpdateAlpha(bool fadingOut)
    {
        timeIntoFade += Time.deltaTime;
        alpha = timeIntoFade / aceFadeTime;
        if (!fadingOut)
            alpha = 1.0f - alpha;
        alpha = Mathf.Clamp01(alpha);
        aceMaterial.color = new Color(aceMaterial.color.r, aceMaterial.color.g, aceMaterial.color.b, alpha);
        return 0.0f != alpha && 1.0f != alpha;
    }
    IEnumerator MoveCameraContainer()
    {
        updatingCameraPosition = true;
        bool destinationReached = false, stillFadingAce = true;
        timeIntoFade = 0.0f;
        if (!usingThirdPersonCamera)
        {
            ACE.SetActive(true);
            eyeRayCaster.rayCheckLength += (firstPersonAnchor.position - thirdPersonAnchor.position).magnitude;
        }
        while (!destinationReached)
        {
            if (usingThirdPersonCamera)
            {
                cameraContainerTransform.position = Vector3.Lerp(cameraContainerTransform.position, firstPersonAnchor.position, Time.deltaTime * translationRate);
                if (Vector3.Distance(cameraContainerTransform.position, firstPersonAnchor.position) < tolerance)
                    destinationReached = true;
            }
            else
            {
                cameraContainerTransform.position = Vector3.Lerp(cameraContainerTransform.position, thirdPersonAnchor.position, Time.deltaTime * translationRate);
                if (Vector3.Distance(cameraContainerTransform.position, thirdPersonAnchor.position) < tolerance)
                    destinationReached = true;
            }
            if (stillFadingAce)
                stillFadingAce = UpdateAlpha(!usingThirdPersonCamera);
            yield return null;
        }
        if (usingThirdPersonCamera)
        {
            ACE.SetActive(false);
            eyeRayCaster.rayCheckLength = originalEyeRaycastLength;
        }
        usingThirdPersonCamera = !usingThirdPersonCamera;
        updatingCameraPosition = false;
    }
}