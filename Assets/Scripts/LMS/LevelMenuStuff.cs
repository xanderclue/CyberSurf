using UnityEngine;
public class LevelMenuStuff : MonoBehaviour
{
    [SerializeField] private GameObject menuBox = null;
    [SerializeField] private EnterLevelOptionsButton enterLevelOptions = null;
    [SerializeField] private LevelMenuButtons levelMenuButtons = null;
    [SerializeField] private LevelSelectOptions levelSelectOptions = null;
    [SerializeField] private RaceModeOptions raceModeOptions = null;
    [SerializeField] private LapsOptions lapsOptions = null;
    [SerializeField] private AiOptions aiOptions = null;
    [SerializeField] private DifficultyOptions difficultyOptions = null;
    [SerializeField] private WeatherOptions weatherOptions = null;
    [SerializeField] private TimeOfDayOptions timeOfDayOptions = null;
    [SerializeField] private MirrorTrackOptions mirrorTrackOptions = null;
    [SerializeField] private ReverseTrackOptions reverseTrackOptions = null;

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