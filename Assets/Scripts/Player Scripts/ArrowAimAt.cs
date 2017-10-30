using UnityEngine;
public class ArrowAimAt : MonoBehaviour
{
    [SerializeField] private Transform[] thingsToLookAt = null;
    private int currentlyLookingAt = -1;
    private void Update()
    {
        if (currentlyLookingAt >= 0 && currentlyLookingAt < thingsToLookAt.Length)
            transform.LookAt(thingsToLookAt[currentlyLookingAt].transform);
    }
}