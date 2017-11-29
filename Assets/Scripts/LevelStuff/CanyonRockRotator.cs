using UnityEngine;
public class CanyonRockRotator : MonoBehaviour
{
    [SerializeField] private float angularVelocityLimit = 10.0f, xTorqueAmount = 10.0f, yTorqueAmount = 10.0f, zTorqueAmount = 10.0f;
    private Rigidbody rockRigidBody = null;
    private void Start()
    {
        rockRigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (rockRigidBody.angularVelocity.magnitude < angularVelocityLimit)
            rockRigidBody.AddTorque(xTorqueAmount, yTorqueAmount, zTorqueAmount);
    }
}