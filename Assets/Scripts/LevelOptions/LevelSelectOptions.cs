using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Xander.NullConversion;
using Xander.Debugging;
public class LevelSelectOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField] private TextMeshPro levelNameText = null;
    [SerializeField] private Image levelImage = null;
    [Space, Header("Level Images")] [SerializeField] private Sprite canyonImage = null;
    [SerializeField] private Sprite multiEnrironmentImage = null;
    [SerializeField] private Sprite backyardRacetrackImage = null;
    private enum Level { Canyon, MultiEnvironment, BackyardRacetrack, NumLevels }
    [SerializeField] private Level defaultLevel = Level.Canyon;
    private Level tempLevel;
    [SerializeField] private WorldPortalProperties portal = null;
    public const int LevelBuildOffset = 2;
    public const int LevelCount = (int)(Level.NumLevels);
    public const int MirroredOffset = LevelCount;
    public const int MirroredBuildOffset = LevelBuildOffset + MirroredOffset;
    public const int ReversedOffset = MirroredOffset + LevelCount;
    public const int ReversedBuildOffset = LevelBuildOffset + ReversedOffset;
    public const int MirroredReversedOffset = ReversedOffset + LevelCount;
    public const int MirroredReversedBuildOffset = LevelBuildOffset + MirroredReversedOffset;
    public static int GetLevelOffset
    {
        get
        {
            if (LevelManager.mirrorMode)
                if (LevelManager.reverseMode)
                    return MirroredReversedOffset;
                else
                    return MirroredOffset;
            else if (LevelManager.reverseMode)
                return ReversedOffset;
            return 0;
        }
    }
    public static int GetBuildOffset { get { return LevelBuildOffset + GetLevelOffset; } }
    new private void Start()
    {
        base.Start();
        if (null == portal)
        {
            Debug.LogWarning("Missing LevelSelectOptions.portal.. Will attempt to find a world portal" + this.Info(), this);
            portal = FindObjectOfType<WorldPortalText>().ConvertNull()?.GetNullConvertedComponent<WorldPortalProperties>() ?? FindObjectOfType<WorldPortalProperties>();
            if (null == portal)
                Debug.LogWarning("LevelSelectOptions cannot find portal" + this.Info(), this);
        }
    }
    private void OnEnable()
    {
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
        if (0 == tempLevel)
            tempLevel = Level.NumLevels - 1;
        else
            --tempLevel;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        ++tempLevel;
        if (Level.NumLevels == tempLevel)
            tempLevel = 0;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        levelNameText.SetText(tempLevel.ToString());
        switch (tempLevel)
        {
            case Level.Canyon:
                levelImage.sprite = canyonImage;
                break;
            case Level.MultiEnvironment:
                levelImage.sprite = multiEnrironmentImage;
                break;
            case Level.BackyardRacetrack:
                levelImage.sprite = backyardRacetrackImage;
                break;
            default:
                Debug.LogWarning("Switch statement on Level enum tempLevel in LevelSelectOptions.cs is missing case for Level." + tempLevel.ToString() + this.Info(), this);
                break;
        }
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        portal.SceneIndex = (int)tempLevel + LevelBuildOffset;
    }
    public override void DefaultOptions()
    {
        base.DefaultOptions();
        tempLevel = defaultLevel;
        UpdateDisplay();
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempLevel = (Level)(portal.SceneIndex - LevelBuildOffset);
        UpdateDisplay();
    }
}