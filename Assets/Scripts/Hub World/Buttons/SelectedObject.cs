using UnityEngine;
using Xander.Debugging;
using Xander.NullConversion;
public abstract class SelectedObject : MonoBehaviour
{
    [Multiline] public string tooltipText = "";
    [SerializeField, Tooltip("\"Time To Wait\": How long it takes for the reticle to fill up (measured in FixedUpdate ticks)")] private int timeToWait = 50;
    [SerializeField, Tooltip("\"Delay\": How long to stare at the button before it registers that you want to select it (measured in FixedUpdate ticks) [-1 sets it to default 20]")] private int delay = DEFAULT_DELAY;
    [SerializeField, Tooltip("\"Wait Time\": How long to wait before it can select again while staring at the button (measured in seconds)")] private float waitTime = 1.5f;
    private int timeWaited = 0, delayTime = 0;
    private static AudioClip loadedSuccessSound = null, loadedSelectedSound = null;
    protected AudioClip successSound = null;
    protected AudioClip selectedSound = null;
    private const int DEFAULT_DELAY = 20;
    private float waitTimer = 0.0f;
    private bool CanSelect { get { return waitTimer <= 0.0f; } set { waitTimer = value ? 0.0f : waitTime; } }
    private bool isSelected = false, isDisabled = false, selectsoundplayed = false;
    public bool tooltipOnly = false;
    public bool IsDisabled { set { isDisabled = value; if (isDisabled && null != theReticle) theReticle.UpdateReticleFill(0.0f); } }
    protected float WaitTime { get { return waitTime; } }
    private reticle theReticle = null;
    public const string LAYERNAME = "Selectable";
    public static int Selectable_Layer { get { return LayerMask.NameToLayer(LAYERNAME); } }
    protected virtual void SelectedFunction() { }
    protected virtual void DeselectedFunction() { }
    protected abstract void SuccessFunction();
    protected void Awake()
    {
        if (tooltipOnly)
            SetupTooltipOnly();
        else if (delay < 0)
            delay = DEFAULT_DELAY;
    }
    public void Selected(reticle grabbedReticle)
    {
        if (enabled && CanSelect && !isDisabled)
        {
            if (gameObject.activeSelf)
                TooltipTextScript.SetText(tooltipText);
            SelectedFunction();
            if (!tooltipOnly)
            {
                theReticle = grabbedReticle;
                isSelected = true;
            }
        }
    }
    public void Deselected()
    {
        TooltipTextScript.SetText("");
        if (enabled)
        {
            DeselectedFunction();
            if (!tooltipOnly)
            {
                theReticle.ConvertNull()?.UpdateReticleFill(0.0f);
                isSelected = false;
                timeWaited = 0;
                delayTime = 0;
                CanSelect = true;
            }
        }
    }
    protected void FixedUpdate()
    {
        if (tooltipOnly)
            return;
        if (isSelected && CanSelect && !isDisabled)
        {
            ++delayTime;
            if (delayTime >= delay)
                ++timeWaited;
            if (timeWaited >= timeToWait)
            {
                SuccessFunction();
                CanSelect = false;
                theReticle.UpdateReticleFill(0.0f);
                timeWaited = 0;
                if (isActiveAndEnabled && null != successSound)
                    AudioSource.PlayClipAtPoint(successSound, transform.position, AudioManager.SfxVolume);
            }
            theReticle.UpdateReticleFill((float)timeWaited / timeToWait);
            if (!selectsoundplayed && timeWaited >= 2)
            {
                if (null != selectedSound)
                    AudioSource.PlayClipAtPoint(selectedSound, transform.position, AudioManager.SfxVolume);
                selectsoundplayed = true;
            }
        }
        else
            selectsoundplayed = false;
    }
    protected void Start()
    {
        if (Selectable_Layer != gameObject.layer)
        {
            Debug.LogWarning("A SelectedObject is not in " + LAYERNAME + " layer. (\"" + gameObject.HierarchyPath() + "\") Changing layer from " +
                LayerMask.LayerToName(gameObject.layer) + " (" + gameObject.layer + ") to " + LAYERNAME + " (" + Selectable_Layer + ")" + this.Info(), this);
            gameObject.layer = Selectable_Layer;
        }
        if (null == GetComponent<Collider>())
            Debug.LogWarning("A SelectedObject script is attached to an object that does not have a Collider component. (\"" + gameObject.HierarchyPath() + "\")" + this.Info(), this);
        if (!tooltipOnly)
        {
            if (null == loadedSuccessSound) loadedSuccessSound = Resources.Load<AudioClip>("Sounds/ButtonSuccess");
            if (null == loadedSelectedSound) loadedSelectedSound = Resources.Load<AudioClip>("Sounds/ButtonSelect");
            successSound = loadedSuccessSound;
            selectedSound = loadedSelectedSound;
        }
    }
    protected void Update()
    {
        if (!tooltipOnly)
            waitTimer -= Time.deltaTime;
    }
    public void SetupTooltipOnly()
    {
        delay = int.MaxValue;
        waitTime = float.MaxValue;
        timeToWait = int.MaxValue;
        selectedSound = null;
        successSound = null;
    }
}