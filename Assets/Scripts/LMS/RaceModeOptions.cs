using UnityEngine;
using TMPro;
public class RaceModeOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField]
    private TextMeshPro raceModeText = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing RaceModeOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing RaceModeOptions.rightButton");
        if (null == raceModeText)
            Debug.LogWarning("Missing RaceModeOptions.raceModeText");
    }
    new private void OnEnable()
    {
        base.OnEnable();
        leftButton.OnButtonPressed += ButtonLeftFunction;
        rightButton.OnButtonPressed += ButtonRightFunction;
    }
    private void OnDisable()
    {
        leftButton.OnButtonPressed -= ButtonLeftFunction;
        rightButton.OnButtonPressed -= ButtonRightFunction;
    }
    private void ButtonLeftFunction()
    {
    }
    private void ButtonRightFunction()
    {
    }
}