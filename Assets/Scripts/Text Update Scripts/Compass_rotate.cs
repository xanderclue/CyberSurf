using UnityEngine;
using UnityEngine.UI;
public class Compass_rotate : MonoBehaviour
{
    [SerializeField] private Vector3 thingToLookAt = Vector3.zero;
    public Camera the_camera;
    //private Transform player = null;
    private Slider myself = null;
    private float angle = 0.0f, pointPosition = 0.5f;
    private void Start()
    {
       // player = GameManager.player.transform;
        myself = GetComponent<Slider>();
    }
    private void Update()
    {
        angle = Vector3.SignedAngle(the_camera.transform.forward, thingToLookAt - the_camera.transform.position, Vector3.up);
        if (angle < -90.0f)
            pointPosition = 0.0f;
        else if (-90.0f < angle && angle < 0.0f)
            pointPosition = 0.5f + angle / 180.0f;
        else if (0.0f < angle && angle < 90.0f)
            pointPosition = 0.5f + angle / 180.0f;
        else if (angle < 180.0f)
            pointPosition = 1;
        myself.value = pointPosition;
        Debug.DrawRay(the_camera.transform.position, the_camera.transform.forward * 100.0f, Color.blue);
    }
}