using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraCounterRotate : MonoBehaviour
{
    Transform cameraContainerTransform;

    private void Start()
    {
		cameraContainerTransform = GetComponent<Transform> ();

        if (VRDevice.isPresent)
			StartCoroutine(WaitForFirstFrame());
    }

	IEnumerator WaitForFirstFrame()
	{
		yield return new WaitForEndOfFrame ();

		StartCoroutine (CounterRotate ());
	}

    IEnumerator CounterRotate()
    {
        if (cameraContainerTransform.eulerAngles.x != 0f)
            cameraContainerTransform.Rotate(Vector3.right, 0f);

        //print("MAIN CAMERA EULER ANGLES: " + mainCameraTransform.eulerAngles);
        //print("BOARD EULER ANGLES: " + boardTransform.eulerAngles);

        yield return null;

        StartCoroutine(CounterRotate());
    }
}
