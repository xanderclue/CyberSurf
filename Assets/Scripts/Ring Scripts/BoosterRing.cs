using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterRing : MonoBehaviour
{
    [SerializeField] private MeshRenderer arrowRenderer;
    [SerializeField] private Transform directionTransform;
    [SerializeField] private float boostAmount = 3.0f;
    [SerializeField] private float boostLength = 0.25f;
    private float timeIntoBoost = 0.0f;
    private Rigidbody rb;

    private void Start()
    {
        arrowRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ("Player" == other.tag)
        {
            rb = other.GetComponent<Rigidbody>();
            StartCoroutine(BoostCoroutine());
        }
    }

    IEnumerator BoostCoroutine()
    {
        while (timeIntoBoost < boostLength)
        {
            yield return new WaitForFixedUpdate();
            timeIntoBoost += Time.deltaTime;
            rb.AddForce(directionTransform.forward * boostAmount, ForceMode.Impulse);
        }
        timeIntoBoost = 0.0f;
        rb = null;
    }
}