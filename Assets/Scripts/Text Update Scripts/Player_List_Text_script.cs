using TMPro;
using UnityEngine;
public class Player_List_Text_script : MonoBehaviour { 



    private TextMeshProUGUI element = null;
    private int recent_position;
    [SerializeField]
    public Vector3[] Ring_path;
    GameObject the_player;
    private AI_Race_Mode_Script[] racers;
    public int checkpoint = 0;
    private static Lap_Text_script laptext = null;
    private void Start()
    {
        if (null == laptext) laptext = FindObjectOfType<Lap_Text_script>();
        element = GetComponent<TextMeshProUGUI>();
        racers = FindObjectsOfType<AI_Race_Mode_Script>();
        the_player = GameObject.FindGameObjectWithTag("Player");
        Ring_path = racers[0].Ring_path;
    }
    private void FixedUpdate()
    {
        if (Ring_path.Length == 0)
        {
            Ring_path = racers[0].Ring_path;
        }
        if (laptext.CurrLap < racers[0].laps)
        {
            element.SetText("AI" + '\n' + "Player");
        }
        else if (laptext.CurrLap == racers[0].laps)
        {
            int temp = 0;
            while (checkpoint == 0 && temp != Ring_path.Length)
            {
                float checkx = the_player.transform.position.x - Ring_path[temp].x;
                float checky = the_player.transform.position.y - Ring_path[temp].y;
                float checkz = the_player.transform.position.z - Ring_path[temp].z;
                if (checkx < 8 && checkx > -8 && checky < 8 && checky > -8 && checkz < 8 && checkz > -8)
                {
                    checkpoint = temp;
                    recent_position = temp;
                }
                temp++;
            }
            if (recent_position < racers[0].counter)
            {
                element.SetText("AI" + '\n' + "Player");
                checkpoint = 0;
            }

            else
            {
                element.SetText("Player" + '\n' + "AI");
                checkpoint = 0;
            }
        }
        else
        {
            element.SetText("Player" + '\n' + "AI");
            checkpoint = 0;
        }
    }
}