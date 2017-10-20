using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanyonRockRotator : MonoBehaviour
{
    Rigidbody rockRigidBody;

    [SerializeField] float angularVelocityLimit = 10f;
    [SerializeField] float xTorqueAmount = 10f;
    [SerializeField] float yTorqueAmount = 10f;
    [SerializeField] float zTorqueAmount = 10f;

    Vector3 torqueVector;

	void Start ()
    {
        rockRigidBody = GetComponent<Rigidbody>();
        torqueVector = new Vector3(xTorqueAmount, yTorqueAmount, zTorqueAmount);
	}

    private void FixedUpdate()
    {
        if (rockRigidBody.angularVelocity.magnitude < angularVelocityLimit)
            rockRigidBody.AddTorque(torqueVector);
    }
}
