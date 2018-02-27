using UnityEngine;
public class startupSplashScreenPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] splashScreens = null;
    [SerializeField] private float timeToPlayScreen = 8.0f;
    [SerializeField]
    private GameObject backPlane = null;
    [SerializeField] private GameObject tutorialObject = null;
    [SerializeField] private EventSelectedObject ConfirmButton = null;
    [SerializeField] private EventSelectedObject NextButton = null;
    [SerializeField] private EventSelectedObject SkipButton = null;
    private static float timePlayingCurrent = 0.0f;
    private static int currentScreen = 0;
    private void Start()
    {
        backPlane.SetActive(false);

        if (currentScreen < splashScreens.Length)
        {
            for (int i = 1; i < splashScreens.Length; ++i)
                splashScreens[i].SetActive(false);
            ConfirmButton.gameObject.SetActive(true);
        }
        else Destroy(tutorialObject);
    }
    private void OnNextAction()
    {
        timePlayingCurrent = 0.0f;
        ++currentScreen;
    }
    private void OnSkipAction()
    {
        currentScreen = splashScreens.Length;
    }
    private void OnEnable()
    {
        ConfirmButton.OnSelectSuccess += OnNextAction;
        NextButton.OnSelectSuccess += OnNextAction;
        SkipButton.OnSelectSuccess += OnSkipAction;
    }
    private void OnDisable()
    {
        ConfirmButton.OnSelectSuccess -= OnNextAction;
        NextButton.OnSelectSuccess -= OnNextAction;
        SkipButton.OnSelectSuccess -= OnSkipAction;
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < splashScreens.Length; ++i)
        {
            splashScreens[i].SetActive(i == currentScreen);
        }
        if (currentScreen == 0)
        {
            respawnAndDespawnSphere.SphereState = false;
            ConfirmButton.gameObject.SetActive(true);
            NextButton.gameObject.SetActive(false);
            SkipButton.gameObject.SetActive(false);
        }
        else if (currentScreen == splashScreens.Length - 1)
        {
            NextButton.transform.position = ConfirmButton.transform.position;
            ConfirmButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
            SkipButton.gameObject.SetActive(false);
            timePlayingCurrent = 0;
        }
        else if (currentScreen < splashScreens.Length)
        {

            ConfirmButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
            SkipButton.gameObject.SetActive(true);
            timePlayingCurrent += Time.fixedDeltaTime;
            if (timePlayingCurrent >= timeToPlayScreen)
            {
                splashScreens[currentScreen].SetActive(false);
                ++currentScreen;
                if (currentScreen < splashScreens.Length)
                    splashScreens[currentScreen].SetActive(true);
                timePlayingCurrent = 0.0f;
            }
        }
        else
        {
            keepPlayerStill.tutorialOn = false;
            respawnAndDespawnSphere.SphereState = true;
            Destroy(tutorialObject);
            return;
        }
    }
}