using UnityEngine;
public class DifficultyOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton leftButton = null, rightButton = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing DifficultyOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing DifficultyOptions.rightButton");
    }
}