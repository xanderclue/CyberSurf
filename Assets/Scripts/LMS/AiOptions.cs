using UnityEngine;
public class AiOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton plusButton = null, minusButton = null;
    new private void Start()
    {
        base.Start();
    }
}