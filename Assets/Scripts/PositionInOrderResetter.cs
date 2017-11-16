using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInOrderResetter : MonoBehaviour
{
    [SerializeField] float timeToActivate = 15f;
    float timer = 0f;
    bool isActive = false;
    int maxLap = 0, currLap = 1;

    public void Setup(int maxLap)
    {
        this.maxLap = maxLap;
    }

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
        if (isActive && other.tag == "Player" && currLap < maxLap)
        {
            RingScoreScript.PrevPositionInOrder = 0;

            timer = 0f;
            isActive = false;
            ++currLap;
        }
    }
}
