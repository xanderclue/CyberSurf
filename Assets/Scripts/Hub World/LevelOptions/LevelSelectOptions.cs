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
    [SerializeField] private LevelManager.Level defaultLevel = LevelManager.Level.Canyon;
    private LevelManager.Level tempLevel;
    [SerializeField] private WorldPortalProperties portal = null;
    public const int LevelBuildOffset = 2;
    public const int HubWorldBuildIndex = LevelBuildOffset - 1;
    public const int LevelCount = (int)(LevelManager.Level.NumLevels);
    public const int MirroredOffset = LevelCount;
    public const int MirroredBuildOffset = LevelBuildOffset + MirroredOffset;
    public const int ReversedOffset = MirroredOffset + LevelCount;
    public const int ReversedBuildOffset = LevelBuildOffset + ReversedOffset;
    public const int MirroredReversedOffset = ReversedOffset + LevelCount;
    public const int MirroredReversedBuildOffset = LevelBuildOffset + MirroredReversedOffset;
    public static int GetLevelOffset => LevelManager.mirrorMode ? (LevelManager.reverseMode ? MirroredReversedOffset : MirroredOffset) : (LevelManager.reverseMode ? ReversedOffset : 0);
    public static int GetBuildOffset => LevelBuildOffset + GetLevelOffset;
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
            tempLevel = LevelManager.Level.NumLevels - 1;
        else
            --tempLevel;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        ++tempLevel;
        if (LevelManager.Level.NumLevels == tempLevel)
            tempLevel = 0;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        levelNameText.SetText(tempLevel.ToString());
        levelImage.sprite = LevelManager.GetLevelPreview(tempLevel);
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
        tempLevel = (LevelManager.Level)(portal.SceneIndex - LevelBuildOffset);
        UpdateDisplay();
    }
}