using UnityEngine;
public class WeatherOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton leftButton = null, rightButton = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing WeatherOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing WeatherOptions.rightButton");
    }
}