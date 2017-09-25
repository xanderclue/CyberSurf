using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBouncer : MonoBehaviour
{
    enum StartDirection { Up, Down };

    Transform anchor;
    float direction = 1f;
    float originalHeight;

    [SerializeField] StartDirection startDirection = StartDirection.Up;
    [SerializeField] float bounceDistance = 1f;
    [SerializeField] float bounceRate = 5f;

	void Start ()
    {
        if (startDirection == StartDirection.Down)
            direction = -1f;

        anchor = GetComponent<Transform>();
        originalHeight = anchor.position.y;

        if (GetComponent<MeshRenderer>().enabled == true)
            GetComponent<MeshRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (direction > 0f)
        {
            if (!(anchor.position.y < originalHeight + bounceDistance))
                direction *= -1f;
        }
        else
        {
            if (!(anchor.position.y > originalHeight - bounceDistance))
                direction *= -1f;
        }

        anchor.Translate(Vector3.up * bounceRate * direction * Time.deltaTime, Space.World);
    }
}
