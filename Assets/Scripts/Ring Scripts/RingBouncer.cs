using UnityEngine;

public class RingBouncer : MonoBehaviour
{
    enum StartDirection { Positive, Negative };

    private Transform anchor;
    private bool negativeDirection = false;

    private Vector3 maxPos, minPos, currPos;

    [SerializeField] private StartDirection startDirection = StartDirection.Positive;
    [SerializeField] private bool bounceVertically = true;
    [SerializeField] private float bounceDistance = 1.0f;
    [SerializeField] private float bounceRate = 5.0f;

    void Start()
    {
        if (StartDirection.Negative == startDirection)
            negativeDirection = true;

        anchor = transform;

        if (bounceVertically)
        {
            maxPos = anchor.position + anchor.TransformDirection(Vector3.up * bounceDistance);
            minPos = anchor.position - anchor.TransformDirection(Vector3.up * bounceDistance);
        }
        else
        {
            maxPos = anchor.position + anchor.TransformDirection(Vector3.left * bounceDistance);
            minPos = anchor.position - anchor.TransformDirection(Vector3.left * bounceDistance);
        }

        currPos = anchor.position;
    }

    private void FixedUpdate()
    {
        if (negativeDirection)
        {
            if (currPos == minPos)
                negativeDirection = false;

            anchor.position = currPos = Vector3.MoveTowards(currPos, minPos, Time.fixedDeltaTime * bounceRate);
        }
        else
        {
            if (currPos == maxPos)
                negativeDirection = true;

            anchor.position = currPos = Vector3.MoveTowards(currPos, maxPos, Time.fixedDeltaTime * bounceRate);
        }
    }
}