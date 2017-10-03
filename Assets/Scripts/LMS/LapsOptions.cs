using UnityEngine;
public class LapsOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton plusButton = null, minusButton = null;
    new private void Start()
    {
        base.Start();
    }
}