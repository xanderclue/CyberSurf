using UnityEngine;
using TMPro;
public class WeatherOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField]
    private TextMeshPro weatherText = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing WeatherOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing WeatherOptions.rightButton");
        if (null == weatherText)
            Debug.LogWarning("Missing WeatherOptions.weatherText");
    }
}