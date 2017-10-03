using UnityEngine;
public class AiOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton plusButton = null, minusButton = null;
    new private void Start()
    {
        base.Start();
        if (null == plusButton)
            Debug.LogWarning("Missing AiOptions.plusButton");
        if (null == minusButton)
            Debug.LogWarning("Missing AiOptions.minusButton");
    }
}