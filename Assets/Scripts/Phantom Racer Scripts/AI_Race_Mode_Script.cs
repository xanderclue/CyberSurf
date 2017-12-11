using UnityEngine;
public class AI_Race_Mode_Script : MonoBehaviour
{
    public Vector3[] Ring_path = null;
    [SerializeField] private Vector3 Joe;
    public int counter = 0;
    public float wait = 0.0f;
    private void Start()
    {
        if (GameMode.Race == GameManager.gameMode)
        {
            Joe = transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        Vector3 the_goal = Ring_path[counter];
        Joe.x = Mathf.MoveTowards(Joe.x, the_goal.x, 0.5f);
        Joe.y = Mathf.MoveTowards(Joe.y, the_goal.y, 0.5f);
        Joe.z = Mathf.MoveTowards(Joe.z, the_goal.z, 0.5f);
        transform.position = Joe;
        float checkx, checky, checkz;
        checkx = Joe.x - the_goal.x;
        checky = Joe.y - the_goal.y;
        checkz = Joe.z - the_goal.z;
        if (checkx < 5.0f && checkx > -5.0f && checky < 5.0f && checky > -5.0f && checkz < 5.0f && checkz > -5.0f)
        {
            {
                ++counter;
                wait = 0;
                if (counter >= Ring_path.Length)
                {
                    counter = 0;
                }
            }
        }
        else
        {
            wait += 0.1f;
        }
    }
}