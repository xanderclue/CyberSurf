using UnityEngine;
public class LevelMenuStuff : MonoBehaviour
{
    [SerializeField] private GameObject menuBox = null;
    [SerializeField] private EnterLevelOptionsButton enterLevelOptions = null;
    public LevelMenuButtons levelMenuButtons = null;
    public LevelSelectOptions levelSelectOptions = null;
    public RaceModeOptions raceModeOptions = null;
    public LapsOptions lapsOptions = null;
    public AiOptions aiOptions = null;
    public DifficultyOptions difficultyOptions = null;
    public WeatherOptions weatherOptions = null;
    public TimeOfDayOptions timeOfDayOptions = null;
    public MirrorTrackOptions mirrorTrackOptions = null;
    public ReverseTrackOptions reverseTrackOptions = null;

    private void Start()
    {
        if (null == menuBox)
            for (int i = 0; i < transform.childCount; ++i)
                if ("MenuBox" == transform.GetChild(i).name)
                {
                    menuBox = transform.GetChild(i).gameObject;
                    break;
                }
        if (null == enterLevelOptions)
            enterLevelOptions = GetComponentInChildren<EnterLevelOptionsButton>();
        if (null == levelMenuButtons)
            levelMenuButtons = GetComponentInChildren<LevelMenuButtons>();
        if (null == levelSelectOptions)
            levelSelectOptions = GetComponentInChildren<LevelSelectOptions>();
        if (null == raceModeOptions)
            raceModeOptions = GetComponentInChildren<RaceModeOptions>();
        if (null == lapsOptions)
            lapsOptions = GetComponentInChildren<LapsOptions>();
        if (null == aiOptions)
            aiOptions = GetComponentInChildren<AiOptions>();
        if (null == difficultyOptions)
            difficultyOptions = GetComponentInChildren<DifficultyOptions>();
        if (null == weatherOptions)
            weatherOptions = GetComponentInChildren<WeatherOptions>();
        if (null == timeOfDayOptions)
            timeOfDayOptions = GetComponentInChildren<TimeOfDayOptions>();
        if (null == mirrorTrackOptions)
            mirrorTrackOptions = GetComponentInChildren<MirrorTrackOptions>();
        if (null == reverseTrackOptions)
            reverseTrackOptions = GetComponentInChildren<ReverseTrackOptions>();
        menuBox.SetActive(false);
        enterLevelOptions.gameObject.SetActive(true);
    }
    public void EnterMenu()
    {
        menuBox.SetActive(true);
        enterLevelOptions.gameObject.SetActive(false);
    }
    public void ExitMenu()
    {
        menuBox.SetActive(false);
        enterLevelOptions.gameObject.SetActive(true);
    }
}