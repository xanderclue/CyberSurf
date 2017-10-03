using UnityEngine;
public class ReverseTrackOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton onButton = null, offButton = null;
    new private void Start()
    {
        base.Start();
        if (null == onButton)
            Debug.LogWarning("Missing ReverseTrackOptions.onButton");
        if (null == offButton)
            Debug.LogWarning("Missing ReverseTrackOptions.offButton");
    }
}