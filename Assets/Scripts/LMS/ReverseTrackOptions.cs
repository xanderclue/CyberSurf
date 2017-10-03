using UnityEngine;
public class ReverseTrackOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton onButton = null, offButton = null;
    new private void Start()
    {
        base.Start();
    }
}