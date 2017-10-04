using UnityEngine;
using TMPro;
public class LevelSelectOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField]
    private TextMeshPro levelNameText = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing LevelSelectOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing LevelSelectOptions.rightButton");
        if (null == levelNameText)
            Debug.LogWarning("Missing LevelSelectOptions.levelNameText");
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