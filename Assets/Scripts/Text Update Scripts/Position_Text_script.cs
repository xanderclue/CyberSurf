using TMPro;
using UnityEngine;
public class Position_Text_script : MonoBehaviour
{
    private TextMeshProUGUI element = null;
    [SerializeField] public Vector3[] Ring_path;
    private AI_Race_Mode_Script[] racers;
    private int checkpoint = 0;
    private static Lap_Text_script laptext = null;
    private void Start()
    {
        if (null == laptext) laptext = FindObjectOfType<Lap_Text_script>();
        element = GetComponent<TextMeshProUGUI>();
        racers = FindObjectsOfType<AI_Race_Mode_Script>();
        Ring_path = racers[0].Ring_path;
    }
    private void Update()
    {
        if (laptext.CurrLap < racers[0].laps)
        {
            element.SetText("2nd");
        }
        else
        {
            element.SetText("1st");
        }
    }
}
