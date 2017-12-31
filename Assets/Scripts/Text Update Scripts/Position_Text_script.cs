using TMPro;
using UnityEngine;
public class Position_Text_script : MonoBehaviour
{
    private TextMeshProUGUI element = null;
    [SerializeField] public Vector3[] Ring_path;
    private AI_Race_Mode_Script[] racers;
    private int checkpoint = 0;
    private void Start()
    {
        element = GetComponent<TextMeshProUGUI>();
        racers = FindObjectsOfType<AI_Race_Mode_Script>();
        Ring_path = racers[0].Ring_path;
    }
    private void Update()
    {
        element.SetText("1st");
       
    }
}
