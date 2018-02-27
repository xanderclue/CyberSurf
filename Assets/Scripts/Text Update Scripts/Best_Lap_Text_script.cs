using UnityEngine;
using TMPro;
public class Best_Lap_Text_script : MonoBehaviour
{

    public float[] times;
    private int curr_lap;
    private Lap_Text_script LT;
    private TextMeshProUGUI element = null;

    private void Start()
    {
        element = GetComponent<TextMeshProUGUI>();
        times = new float[5];

    }
    public void Update_text()
    {
        string text_to_write = "Best Lap" + '\n';
        for (int i = 0; i < times.Length; i++)
        {
            text_to_write += times[i].ToString("n2") + '\n';
        }
        element.SetText(text_to_write);
    }
    public void Cleanup()
    {
        for (int i = 0; i < times.Length; i++)
        {
            times[i] = 0.00f;
        }
        Update_text();
    }
}