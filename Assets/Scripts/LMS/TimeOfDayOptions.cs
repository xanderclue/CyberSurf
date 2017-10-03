using UnityEngine;
public class TimeOfDayOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton leftButton = null, rightButton = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing TimeOfDayOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing TimeOfDayOptions.rightButton");
    }
}