using UnityEngine;
public class LapsOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton plusButton = null, minusButton = null;
    new private void Start()
    {
        base.Start();
        if (null == plusButton)
            Debug.LogWarning("Missing LapsOptions.plusButton");
        if (null == minusButton)
            Debug.LogWarning("Missing LapsOptions.minusButton");
    }
}