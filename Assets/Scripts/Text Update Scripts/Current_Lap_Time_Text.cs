using UnityEngine;
using TMPro;
public class Current_Lap_Time_Text : MonoBehaviour {

    private float Time_passed, Current;
    private int curr_lap;
    private Lap_Text_script LT;
    private Best_Lap_Text_script BLT;
    private TextMeshProUGUI element = null;

    private void Start()
    {
        element = GetComponent<TextMeshProUGUI>();
        LT = GameObject.FindObjectOfType<Lap_Text_script>();
        BLT = GameObject.FindObjectOfType<Best_Lap_Text_script>();
        Time_passed = 0;
        curr_lap = 1;
        BLT.Cleanup();
    }
    private void Update()
    {
        Current = RoundTimer.timeInLevel;
        element.SetText((Current - Time_passed).ToString("n2"));
        if (curr_lap != LT.CurrLap)
        {
            BLT.times[curr_lap - 1] = Current - Time_passed;
            curr_lap++;
            BLT.Update_text();
            Time_passed = Current;
        }
        if (curr_lap > LT.CurrLap)
        {
            curr_lap = 1;
            Time_passed = 0;
            BLT.Cleanup();
        }
    }
}