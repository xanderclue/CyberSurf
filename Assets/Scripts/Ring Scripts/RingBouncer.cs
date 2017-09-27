using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBouncer : MonoBehaviour
{
    enum StartDirection { Positive, Negative };

    Transform anchor;
    float direction = 1f;
    float originalPosition;

    [SerializeField] StartDirection startDirection = StartDirection.Positive;
    [SerializeField] bool bounceVertically = true;
    [SerializeField] float bounceDistance = 1f;
    [SerializeField] float bounceRate = 5f;

	void Start ()
    {
        if (startDirection == StartDirection.Negative)
            direction = -1f;

        anchor = GetComponent<Transform>();

        if (bounceVertically)
            originalPosition = anchor.localPosition.y;
        else
            originalPosition = anchor.localPosition.x;

        if (GetComponent<MeshRenderer>().enabled == true)
            GetComponent<MeshRenderer>().enabled = false;
    }

    void BounceVertically()
    {
        if (direction > 0f)
        {
            if (!(anchor.localPosition.y < originalPosition + bounceDistance))
                direction *= -1f;
        }
        else
        {
            if (!(anchor.localPosition.y > originalPosition - bounceDistance))
                direction *= -1f;
        }

        anchor.Translate(Vector3.up * bounceRate * direction * Time.deltaTime, Space.Self);
    }

    void BounceHorizontally()
    {
        if (direction > 0f)
        {
            if (!(anchor.localPosition.x < originalPosition + bounceDistance))
                direction *= -1f;
        }
        else
        {
            if (!(anchor.localPosition.x > originalPosition - bounceDistance))
                direction *= -1f;
        }

        anchor.Translate(Vector3.left * bounceRate * direction * Time.deltaTime, Space.Self);
    }

    private void FixedUpdate()
    {
        if (bounceVertically)
            BounceVertically();
        else
            BounceHorizontally();   
    }
}
