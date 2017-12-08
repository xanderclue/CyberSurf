using UnityEngine;
using UnityEngine.PostProcessing;
using static KeyInputManager.VR;
public class GameManager : MonoBehaviour
{
    public ManagerClasses.GameState gameState = new ManagerClasses.GameState();
    public ManagerClasses.GameMode gameMode = new ManagerClasses.GameMode();
    public ManagerClasses.GameDifficulty gameDifficulty = new ManagerClasses.GameDifficulty();
    public ManagerClasses.RoundTimer roundTimer = new ManagerClasses.RoundTimer();
    [HideInInspector] public int lastPortalBuildIndex = 0;
    [SerializeField] private GameObject playerPrefab = null;
    public static GameManager instance = null;
    public static GameObject player = null;
    [HideInInspector] public ScoreManager scoreScript = null;
    [HideInInspector] public LevelManager levelScript = null;
    [HideInInspector] public BoardManager boardScript = null;
    [SerializeField] private bool disableVignette = false;
    private void Awake()
    {
        Application.runInBackground = true;
        if (null == instance)
            instance = this;
        if (this == instance)
        {
            DontDestroyOnLoad(gameObject);
            InitGame();
        }
        else
            Destroy(gameObject);
    }
    private void OnEnable()
    {
        GameSettings.GetBool("RingPath", ref levelScript.RingPathIsOn);
        GameSettings.GetBool("Respawn", ref scoreScript.respawnEnabled);
        GameSettings.GetBool("DebugSpeed", ref boardScript.debugSpeedEnabled);
        GameSettings.GetBool("MirrorMode", ref LevelManager.mirrorMode);
        GameSettings.GetBool("ReverseMode", ref LevelManager.reverseMode);
        GameSettings.GetEnum("Weather", ref WeatherScript.currentWeather);
        GameSettings.GetEnum("TimeOfDay", ref DayNightScript.currentTimeOfDay);
    }
    private void OnDisable()
    {
        GameSettings.SetBool("RingPath", levelScript.RingPathIsOn);
        GameSettings.SetBool("Respawn", scoreScript.respawnEnabled);
        GameSettings.SetBool("DebugSpeed", boardScript.debugSpeedEnabled);
        GameSettings.SetBool("MirrorMode", LevelManager.mirrorMode);
        GameSettings.SetBool("ReverseMode", LevelManager.reverseMode);
        GameSettings.SetEnum("Weather", WeatherScript.currentWeather);
        GameSettings.SetEnum("TimeOfDay", DayNightScript.currentTimeOfDay);
        GameSettings.SetEnum("GameDifficulty", gameDifficulty.currentDifficulty);
        GameSettings.SetEnum("GameMode", gameMode.currentMode);
    }
    private void InitGame()
    {
        scoreScript = GetComponent<ScoreManager>();
        levelScript = GetComponent<LevelManager>();
        boardScript = GetComponent<BoardManager>();
        DontDestroyOnLoad(player = Instantiate(playerPrefab));
        player.GetComponentInChildren<PostProcessingBehaviour>().profile.vignette.enabled = VRPresent && !disableVignette;
        if (VRPresent)
            gameDifficulty.currentDifficulty = GameDifficulties.Easy;
        GameSettings.GetEnum("GameDifficulty", ref gameDifficulty.currentDifficulty);
        GameSettings.GetEnum("GameMode", ref gameMode.currentMode);
        boardScript.SetupBoardManager(player);
        levelScript.SetupLevelManager(gameState, player, instance);
        scoreScript.SetupScoreManager();
        GetComponent<KeyInputManager>().SetupKeyInputManager(gameState);
    }
    private void OnDestroy() => GameSettings.Save();
    private static bool deleteScores = false;
    public static void DeleteScoresOnExit() => deleteScores = true;
    public static bool DoNotSave => deleteScores;
    private void OnApplicationQuit()
    {
        if (deleteScores)
            try { System.IO.File.Delete(Application.persistentDataPath + "/scores.gd"); }
            catch (System.Exception e) { Debug.LogWarning($"Failed to delete scores file: ({e.Message})"); }
    }
}