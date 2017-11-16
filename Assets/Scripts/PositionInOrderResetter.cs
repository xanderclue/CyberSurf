using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInOrderResetter : MonoBehaviour
{
    [SerializeField] float timeToActivate = 30f;
    float timer = 0f;
    bool isActive = false;

    void Update()
    {
        if (!isActive)
        {
            timer += Time.deltaTime;

            if (timer > timeToActivate)
                isActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.tag == "Player")
        {
            RingScoreScript.PrevPositionInOrder = 0;

            timer = 0f;
            isActive = false;
        }
    }
}
