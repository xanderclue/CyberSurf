using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] GameObject ACE;
    [SerializeField] Transform firstPersonAnchor;
    [SerializeField] Transform thirdPersonAnchor;
    [SerializeField] Transform cameraContainerTransform;
    //might not need this one
    [SerializeField] Transform mainCameraTransform;
    [SerializeField] float translationRate = 2f;
    float aceFadeTime = 1f;
    float tolerance = 0.001f;

    KeyInputManager keyInputManager;
    Material aceMaterial;

    bool updatingCameraPosition, usingThirdPersonCamera;
    float timeIntoFade = 0f;
    float alpha = 0f;

    public bool UpdatingCameraPosition { get { return updatingCameraPosition; } }
    public bool UsingThirdPersonCamera { get { return usingThirdPersonCamera; } }
    public Vector3 ThirdPersonAnchorPos { get { return thirdPersonAnchor.position; } }

    private void Start()
    {
        keyInputManager = GameManager.instance.keyInputScript;

        if (ACE.activeInHierarchy)
            ACE.SetActive(false);       

        aceMaterial = ACE.GetComponent<MeshRenderer>().material;
        aceMaterial.color = new Color(aceMaterial.color.r, aceMaterial.color.g, aceMaterial.color.b, 0f);

        updatingCameraPosition = usingThirdPersonCamera = false;
    }

    public void UpdateThirdPersonCamera()
    {
        //only start our coroutine if we aren't already updating it
        if (!updatingCameraPosition)
            StartCoroutine(MoveCameraContainer());
    }

    
    //helper function
    bool UpdateAlpha(bool fadingOut)
    {
        //we want to update our alpha based off of how far into the fade time we are in
        timeIntoFade += Time.deltaTime;
        alpha = timeIntoFade / aceFadeTime;

        if (fadingOut == false)
            alpha = 1f - alpha;

        alpha = Mathf.Clamp01(alpha);
        aceMaterial.color = new Color(aceMaterial.color.r, aceMaterial.color.g, aceMaterial.color.b, alpha);

        if (alpha == 0f || alpha == 1f)
            return false;
        else
            return true;
    }

    IEnumerator MoveCameraContainer()
    {
        updatingCameraPosition = true;
        bool destinationReached = false;
        bool stillFadingAce = true;

        timeIntoFade = 0f;

        //enable ace if we're switching to the third person camera
        if (!usingThirdPersonCamera)
            ACE.SetActive(true);

        //slowly move our camera into position until the camera reaches it's destination
        while (!destinationReached)
        {
            //set our destination depending on if we are moving to or away from the third person camera
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

        //disable ace if we faded him out
        if (usingThirdPersonCamera)
            ACE.SetActive(false);

        usingThirdPersonCamera = !usingThirdPersonCamera;
        updatingCameraPosition = false;
    }
}
