using UnityEngine;
public class MirrorTrackOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton onButton = null, offButton = null;
    new private void Start()
    {
        base.Start();
        if (null == onButton)
            Debug.LogWarning("Missing MirrorTrackOptions.onButton");
        if (null == offButton)
            Debug.LogWarning("Missing MirrorTrackOptions.offButton");
    }
}