using System.Collections.Generic;
using UnityEngine;

public abstract class SelectedObject : MonoBehaviour
{
    [Multiline]
    public string tooltipText = "";
    [SerializeField, Tooltip("\"Time To Wait\": How long it takes for the reticle to fill up (measured in FixedUpdate ticks)")]
    private int timeToWait = 50;
    private int timeWaited = 0;
    private bool isSelected = false;
    protected AudioClip successSound;
    protected AudioClip selectedSound;
    private const int DEFAULT_DELAY = 20;
    [SerializeField, Tooltip("\"Delay\": How long to stare at the button before it registers that you want to select it (measured in FixedUpdate ticks) [-1 sets it to default 20]")]
    private int delay = DEFAULT_DELAY;
    private int delayTime = 0;
    private float waitTimer = 0.0f;
    [SerializeField, Tooltip("\"Wait Time\": How long to wait before it can select again while staring at the button (measured in seconds)")]
    private float waitTime = 1.5f;
    private bool CanSelect { get { return waitTimer <= 0.0f; } set { waitTimer = value ? 0.0f : waitTime; } }
    private bool isDisabled = false;
    private bool selectsoundplayed = false;
    public bool IsDisabled { set { isDisabled = value; if (isDisabled && null != theReticle) theReticle.UpdateReticleFill(0.0f); } }
    [SerializeField]
    private bool tooltipOnly = false;
    protected void Awake()
    {
        if (tooltipOnly)
            SetupTooltipOnly();
        else if (delay < 0)
            delay = DEFAULT_DELAY;
    }

    //object to update for reticle
    private reticle theReticle;

    //grabs the reticle object to show timer status
    public void Selected(reticle grabbedReticle)
    {
        if (!CanSelect || isDisabled)
            return;
        TooltipTextScript.SetText(tooltipText);
        SelectedFunction();
        if (!tooltipOnly)
        {
            theReticle = grabbedReticle;
            isSelected = true;
        }
    }

    //What the class actually does with the object while selected(if applicable)
    protected virtual void SelectedFunction() { }

    //deals with leftovers from selecting the object when you look away
    public void Deselected()
    {
        TooltipTextScript.SetText("");
        DeselectedFunction();
        if (!tooltipOnly)
        {
            if (null != theReticle)
                theReticle.UpdateReticleFill(0.0f);
            isSelected = false;
            timeWaited = 0;
            delayTime = 0;
            CanSelect = true;
        }
    }

    //Cleans up what the class actually does(if applicable)
    protected virtual void DeselectedFunction() { }

    //what the class actually does when select is successful, inherited class must fill this out
    public abstract void SuccessFunction();

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
                if (isActiveAndEnabled)
                {
                    if (null != successSound)
                        AudioSource.PlayClipAtPoint(successSound, transform.position, AudioLevels.Instance.SfxVolume);
                }
            }
            theReticle.UpdateReticleFill((float)timeWaited / timeToWait);
            if (selectsoundplayed == false && timeWaited >= 2)
            {
                if (null != selectedSound)
                    AudioSource.PlayClipAtPoint(selectedSound, transform.position, AudioLevels.Instance.SfxVolume);
                selectsoundplayed = true;
            }
        }
        else
        {
            selectsoundplayed = false;
        }
    }
    public static string LAYERNAME { get { return "Selectable"; } }
    protected void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer(LAYERNAME))
        {
            Debug.LogWarning("A SelectedObject is not in " + LAYERNAME + " layer. (\"" + BuildDebugger.GetHierarchyName(gameObject) + "\") Changing layer from " +
                LayerMask.LayerToName(gameObject.layer) + " (" + gameObject.layer + ") to " + LAYERNAME + " (" + LayerMask.NameToLayer(LAYERNAME) + ")");
            gameObject.layer = LayerMask.NameToLayer(LAYERNAME);
        }
        if (null == GetComponent<Collider>())
            Debug.LogWarning("A SelectedObject script is attached to an object that does not have a Collider component. (\"" + BuildDebugger.GetHierarchyName(gameObject) + "\")");
        if (!tooltipOnly)
        {
            successSound = (AudioClip)Resources.Load("Sounds/Effects/Place_Holder_LoadSuccess");
            selectedSound = (AudioClip)Resources.Load("Sounds/Effects/Place_Holder_ButtonHit");
        }
    }

    protected void Update()
    {
        if (!tooltipOnly)
            waitTimer -= Time.deltaTime;
    }
    protected void SetupTooltipOnly()
    {
        delay = int.MaxValue;
        waitTime = float.MaxValue;
        timeToWait = int.MaxValue;
        selectedSound = null;
        successSound = null;
    }
}