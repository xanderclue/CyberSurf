using UnityEngine;
public class HudOnOffObject : MonoBehaviour
{
    public HudMenuTab.HudMenuOption menuOption = HudMenuTab.HudMenuOption.OverallHud;
    public EventSelectedObject onButton = null, offButton = null;
    [SerializeField]
    private bool defaultOnOffValue = true;
    private bool tempOnOffValue;
    public bool IsOn { get { return tempOnOffValue; } }
    private static EventSelectedObject NewInactiveEventSelectedObject
    {
        get
        {
            GameObject go = new GameObject("IGNORE_THIS");
            go.SetActive(false);
            go.layer = LayerMask.NameToLayer(SelectedObject.LAYERNAME);
            EventSelectedObject outVal = go.AddComponent<EventSelectedObject>();
            outVal.enabled = false;
            GameObject gop = GameObject.Find("!!_NO_TOUCHY_!!_!!_IGNORE_THESE_!!");
            if (null == gop)
                gop = new GameObject("!!_NO_TOUCHY_!!_!!_IGNORE_THESE_!!");
            go.transform.parent = gop.transform;
            return outVal;
        }
    }
    protected void Awake()
    {
        if (null == onButton)
        {
            Debug.LogWarning("Missing HudOnOffObject.offButton (" + BuildDebugger.GetHierarchyName(gameObject) + ")");
            onButton = NewInactiveEventSelectedObject;
        }
        if (null == offButton)
        {
            Debug.LogWarning("Missing HudOnOffObject.offButton (" + BuildDebugger.GetHierarchyName(gameObject) + ")");
            offButton = NewInactiveEventSelectedObject;
        }
    }
    protected void OnEnable()
    {
        onButton.OnSelectSuccess += TurnOn;
        offButton.OnSelectSuccess += TurnOff;
    }
    protected void OnDisable()
    {
        onButton.OnSelectSuccess -= TurnOn;
        offButton.OnSelectSuccess -= TurnOff;
    }
    public void TurnOn()
    {
        tempOnOffValue = true;
        if (null != OnValueChanged)
            OnValueChanged();
    }
    public void TurnOff()
    {
        tempOnOffValue = false;
        if (null != OnValueChanged)
            OnValueChanged();
    }
    public void DefaultValue()
    {
        if (tempOnOffValue != defaultOnOffValue)
        {
            tempOnOffValue = defaultOnOffValue;
            if (null != OnValueChanged)
                OnValueChanged();
        }
    }
    public void ResetValue()
    {
        if (tempOnOffValue != ActualValue)
        {
            tempOnOffValue = ActualValue;
            if (null != OnValueChanged)
                OnValueChanged();
        }
    }
    public void ConfirmValue()
    {
        ActualValue = tempOnOffValue;
    }
    public delegate void ValueChangedEvent();
    public ValueChangedEvent OnValueChanged;
    private bool ActualValue
    {
        get
        {
            switch (menuOption)
            {
                case HudMenuTab.HudMenuOption.OverallHud:
                    break;
                case HudMenuTab.HudMenuOption.Reticle:
                    break;
                case HudMenuTab.HudMenuOption.Speed:
                    break;
                case HudMenuTab.HudMenuOption.Timer:
                    break;
                case HudMenuTab.HudMenuOption.Score:
                    break;
                case HudMenuTab.HudMenuOption.Players:
                    break;
                case HudMenuTab.HudMenuOption.Compass:
                    break;
                case HudMenuTab.HudMenuOption.Arrow:
                    break;
                case HudMenuTab.HudMenuOption.LapCounter:
                    break;
                case HudMenuTab.HudMenuOption.Position:
                    break;
                case HudMenuTab.HudMenuOption.Opacity:
                    break;
                case HudMenuTab.HudMenuOption.Color:
                    break;
                default:
                    break;
            }
            return false;
        }
        set
        {
            switch (menuOption)
            {
                case HudMenuTab.HudMenuOption.OverallHud:
                    break;
                case HudMenuTab.HudMenuOption.Reticle:
                    break;
                case HudMenuTab.HudMenuOption.Speed:
                    break;
                case HudMenuTab.HudMenuOption.Timer:
                    break;
                case HudMenuTab.HudMenuOption.Score:
                    break;
                case HudMenuTab.HudMenuOption.Players:
                    break;
                case HudMenuTab.HudMenuOption.Compass:
                    break;
                case HudMenuTab.HudMenuOption.Arrow:
                    break;
                case HudMenuTab.HudMenuOption.LapCounter:
                    break;
                case HudMenuTab.HudMenuOption.Position:
                    break;
                case HudMenuTab.HudMenuOption.Opacity:
                    break;
                case HudMenuTab.HudMenuOption.Color:
                    break;
                default:
                    break;
            }
        }
    }
}