using UnityEngine;
using TMPro;

public class HudOptionsOpacityColor : MonoBehaviour
{
    public delegate void ValueChangedEvent();
    public ValueChangedEvent OnValueChanged;
    [SerializeField] private EventSelectedObject opacityMinusButton = null, opacityPlusButton = null;
    [SerializeField] private EventSelectedObject colorPreviousButton = null, colorNextButton = null;
    [SerializeField] private TextMeshPro opacityPercentageText = null;
    [SerializeField] private SpriteRenderer colorPreview = null;
    [SerializeField] private Color[] colors = null;
    private Color DefaultColor { get { return colors[0]; } }
    private int tempColorIndex = 0;
    [SerializeField, Range(0.0f, 1.0f)] private float defaultOpacity = 1.0f;
    [SerializeField] private float opacityIncrement = 0.25f;
    private float tempOpacity = 1.0f;
    private Color ActualColor
    {
        get
        {
            textElementController = textElementController ?? GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            Color ret = textElementController.HudColor;
            ret.a = 0.5f;
            return ret;
        }
        set
        {
            textElementController = textElementController ?? GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            Color ret = value;
            ret.a = textElementController.HudColor.a;
            textElementController.HudColor = ret;
        }
    }
    private float ActualOpacity
    {
        get
        {
            textElementController = textElementController ?? GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            return textElementController.HudColor.a;
        }
        set
        {
            textElementController = textElementController ?? GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            Color ret = textElementController.HudColor;
            ret.a = value;
            textElementController.HudColor = ret;
        }
    }
    public Color ColorPreviewValue { get { return (tempColorIndex < 0) ? ActualColor : colors[tempColorIndex]; } }
    public float OpacityValue { get { return tempOpacity; } }
    private TextElementControllerScript textElementController = null;
    private void Awake()
    {
        if (null == colors || colors.Length <= 0)
        {
            colors = new Color[1];
            colors[0] = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        }
        if (GetIndexOf(ActualColor) < 0)
            ActualColor = DefaultColor;
    }

    private void OnEnable()
    {
        opacityMinusButton.OnSelectSuccess += OpacityMinusFunction;
        opacityPlusButton.OnSelectSuccess += OpacityPlusFunction;
        colorPreviousButton.OnSelectSuccess += ColorPreviousFunction;
        colorNextButton.OnSelectSuccess += ColorNextFunction;
    }

    private void OnDisable()
    {
        opacityMinusButton.OnSelectSuccess -= OpacityMinusFunction;
        opacityPlusButton.OnSelectSuccess -= OpacityPlusFunction;
        colorPreviousButton.OnSelectSuccess -= ColorPreviousFunction;
        colorNextButton.OnSelectSuccess -= ColorNextFunction;
    }

    private void ColorNextFunction()
    {
        ++tempColorIndex;
        if (tempColorIndex >= colors.Length)
            tempColorIndex = 0;
        UpdateDisplay();
    }

    private void ColorPreviousFunction()
    {
        --tempColorIndex;
        if (tempColorIndex < 0)
            tempColorIndex = colors.Length - 1;
        UpdateDisplay();
    }

    private void OpacityPlusFunction()
    {
        tempOpacity = Mathf.Clamp01(tempOpacity + opacityIncrement);
        UpdateDisplay();
    }

    private void OpacityMinusFunction()
    {
        tempOpacity = Mathf.Clamp01(tempOpacity - opacityIncrement);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        colorPreview.color = ColorPreviewValue;
        opacityPercentageText.SetText(Mathf.RoundToInt(tempOpacity * 100.0f).ToString() + "%");
        if (null != OnValueChanged)
            OnValueChanged();
    }

    private int GetIndexOf(Color obj)
    {
        for (int i = 0; i < colors.Length; ++i)
        {
            obj.a = colors[i].a;
            if (obj.Equals(colors[i]))
                return i;
        }
        return -1;
    }

    public void ResetValue()
    {
        tempColorIndex = GetIndexOf(ActualColor);
        tempOpacity = ActualOpacity;
        UpdateDisplay();
    }
    public void DefaultValue()
    {
        tempColorIndex = 0;
        tempOpacity = defaultOpacity;
        UpdateDisplay();
    }
    public void ConfirmValue()
    {
        ActualColor = colors[tempColorIndex];
        ActualOpacity = tempOpacity;
    }
}