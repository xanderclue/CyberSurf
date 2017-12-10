using UnityEngine;
public class PositionInOrderResetter : MonoBehaviour
{
    [SerializeField] private float timeToActivate = 15.0f;
    private float timer = 0.0f;
    private bool isActive = false;
    private int maxLap = 0, currLap = 1;
    public int MaxLap { set { maxLap = value; } }
    private void Update()
    {
        if (isActive)
            return;
        timer += Time.deltaTime;
        if (timer > timeToActivate)
            isActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isActive && currLap < maxLap && "Player" == other.tag)
        {
            RingScoreScript.ResetPrevPositionInOrder();
            timer = 0.0f;
            isActive = false;
            ++currLap;
        }
    }
}