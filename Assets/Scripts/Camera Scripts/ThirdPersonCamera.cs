using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] GameObject ACE;
    Transform thirdPersonCameraTransform;

    KeyInputManager keyInputManager;

    private void Start()
    {
        if (ACE.activeInHierarchy)
            ACE.SetActive(false);

        thirdPersonCameraTransform = GetComponent<Transform>();
        keyInputManager = GameManager.instance.keyInputScript;       
    }

    public void StartThirdPerson()
    {
        ACE.SetActive(true);
    }

    public void StopThirdPerson()
    {
        ACE.SetActive(false);
    }
}
