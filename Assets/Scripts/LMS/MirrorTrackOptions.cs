using UnityEngine;
public class MirrorTrackOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton onButton = null, offButton = null;
    new private void Start()
    {
        base.Start();
    }
}