using UnityEngine;
using TMPro;
public class Lap_Text_script : MonoBehaviour
{ 

private TextMeshProUGUI element = null;
    public int curr_lap, max_lap;
private void Start()
{
    element = GetComponent<TextMeshProUGUI>();
        curr_lap = 1;
        max_lap = 0;
}
private void Update()
{
        if (element.enabled)
        {
            element.SetText("Lap" + '\n' + curr_lap.ToString() + " / " + max_lap.ToString());
        }
    
}
}