using UnityEngine;
using TMPro;
public class Current_Lap_Time_Text : MonoBehaviour {

    float Time_passed, Current;
    int curr_lap;
    private Lap_Text_script LT;
    private TextMeshProUGUI element = null;

    private void Start()
    {
        element = GetComponent<TextMeshProUGUI>();
        LT = GameObject.FindObjectOfType<Lap_Text_script>();
        Time_passed = 0;
        curr_lap = 1;
    }
    private void Update()
    {
        Current = RoundTimer.timeInLevel;
        element.SetText((Current - Time_passed).ToString("n2"));
        if (curr_lap != LT.CurrLap)
        {
            curr_lap++;
            Time_passed = Current;
        }
    }
}