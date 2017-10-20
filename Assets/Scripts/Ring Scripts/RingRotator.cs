using UnityEngine;

//parent this gameobject to any rings that you want to rotate
//don't forget to set the individual ring properties and positions
public class RingRotator : MonoBehaviour
{
    [SerializeField] private float rotateRate = 5.0f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * rotateRate);
    }
}