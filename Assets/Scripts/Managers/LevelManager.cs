using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public enum Level { Canyon, MultiEnvironment, BackyardRacetrack, ComputerChip, City, Venice, NumLevels }
    [HideInInspector] public ManagerClasses.GameState gameState = null;
    private GameManager gameManager = null;
    private ScreenFade screenFade = null;
    private GameObject player = null;
    private PlayerMenuController menuController = null;
    [HideInInspector] public int nextScene = 0;
    [HideInInspector] public bool RingPathIsOn = true;
    public Transform[] spawnPoints = null;
    public static bool mirrorMode = false;
    public static bool reverseMode = false;
    public static Level savedCurrentLevel = Level.Canyon;
    [SerializeField] private Sprite[] levelPreviews = null;
    public static Sprite GetLevelPreview(Level level) => GameManager.instance.levelScript.levelPreviews[(int)level];
    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p, GameManager g)
    {
        player = p;
        gameState = s;
        gameManager = g;
        menuController = p.GetComponent<PlayerMenuController>();
        screenFade = p.GetComponentInChildren<ScreenFade>();
        UnityEngine.Assertions.Assert.AreEqual(SceneManager.sceneCountInBuildSettings, spawnPoints.Length, $"GameManager.instance.levelScript.spawnPoints.Length should be {SceneManager.sceneCountInBuildSettings}.. Actual value: {spawnPoints.Length}");
        UnityEngine.Assertions.Assert.AreEqual(LevelSelectOptions.LevelCount, levelPreviews.Length, $"GameManager.instance.levelScript.levelPreviews.Length should be {LevelSelectOptions.LevelCount}.. Actual value: {levelPreviews.Length}");
    }
    private void DoSceneTransition(int sceneIndex)
    {
        nextScene = sceneIndex;
        EventManager.OnTriggerSelectionLock(true);
        player.GetComponentInChildren<effectController>().disableAllEffects();
        screenFade.StartTransitionFade();
    }
    private void UndoSceneTransitionLocks(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex >= LevelSelectOptions.LevelBuildOffset)
        {
            EventManager.OnSetHudOnOff(true);
            ApplyGamemodeChanges();
            gameState.currentState = GameStates.GamePlay;
        }
        else
        {
            if (gameManager.lastPortalBuildIndex != -1)
                menuController.ToggleMenuMovement(true);
            EventManager.OnSetHudOnOff(false);
            gameState.currentState = GameStates.HubWorld;
            gameManager.scoreScript.score = 0;
            gameManager.scoreScript.ringHitCount = 0;
            gameManager.scoreScript.firstPortal = true;
        }
        player.transform.SetPositionAndRotation(spawnPoints[scene.buildIndex].position, spawnPoints[scene.buildIndex].rotation);
        EventManager.OnTriggerSelectionLock(false);
    }
    private void ApplyGamemodeChanges()
    {
        if (GameModes.Free == gameManager.gameMode.currentMode)
        {
            gameManager.lastPortalBuildIndex = -1;
            EventManager.OnCallSetRingPath(false);
        }
        else
            EventManager.OnCallSetRingPath(RingPathIsOn);
    }
    private void OnEnable()
    {
        EventManager.OnTransition += DoSceneTransition;
        SceneManager.sceneLoaded += UndoSceneTransitionLocks;
    }
    private void OnDisable()
    {
        EventManager.OnTransition -= DoSceneTransition;
        SceneManager.sceneLoaded -= UndoSceneTransitionLocks;
    }
}