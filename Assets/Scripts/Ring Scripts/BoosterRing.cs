using System.Collections;
using UnityEngine;
public class BoosterRing : MonoBehaviour
{
    [SerializeField] private MeshRenderer arrowRenderer = null;
    [SerializeField] private Transform directionTransform = null;
    [SerializeField] private float boostAmount = 3.0f, boostLength = 0.25f;
    private float timeIntoBoost = 0.0f;
    private Rigidbody rb = null;
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
    private IEnumerator BoostCoroutine()
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