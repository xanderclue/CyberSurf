using UnityEngine;
using TMPro;
public class LapsOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton plusButton = null, minusButton = null;
    [SerializeField]
    private TextMeshPro lapsText = null;
    new private void Start()
    {
        base.Start();
        if (null == plusButton)
            Debug.LogWarning("Missing LapsOptions.plusButton");
        if (null == minusButton)
            Debug.LogWarning("Missing LapsOptions.minusButton");
        if (null == lapsText)
            Debug.LogWarning("Missing LapsOptions.lapsText");
    }
    private void OnEnable()
    {
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