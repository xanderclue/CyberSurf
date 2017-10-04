using UnityEngine;
using TMPro;
public class AiOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton plusButton = null, minusButton = null;
    [SerializeField]
    private TextMeshPro aiText = null;
    new private void Start()
    {
        base.Start();
        if (null == plusButton)
            Debug.LogWarning("Missing AiOptions.plusButton");
        if (null == minusButton)
            Debug.LogWarning("Missing AiOptions.minusButton");
        if (null == aiText)
            Debug.LogWarning("Missing AiOptions.aiText");
    }
    new private void OnEnable()
    {
        base.OnEnable();
        plusButton.OnButtonPressed += ButtonPlusFunction;
        minusButton.OnButtonPressed += ButtonMinusFunction;
    }
    private void OnDisable()
    {
        plusButton.OnButtonPressed -= ButtonPlusFunction;
        minusButton.OnButtonPressed -= ButtonMinusFunction;
    }
    private void ButtonPlusFunction()
    {
    }
    private void ButtonMinusFunction()
    {
    }
}