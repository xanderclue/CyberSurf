using UnityEngine;
public class LevelMenuStuff : MonoBehaviour
{
    [SerializeField]
    private GameObject menuBox = null;
    [SerializeField]
    private EnterLevelOptionsButton enterLevelOptions = null;
    [SerializeField]
    private LevelMenuObjectGroup menuButtons = null,
        raceModeOptions = null,
        lapsOptions = null,
        aiOptions = null,
        difficultyOptions = null,
        weatherOptions = null,
        timeOfDayOptions = null,
        mirrorTrackOptions = null,
        reverseTrackOptions = null;

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