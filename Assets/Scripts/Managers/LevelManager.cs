using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    [HideInInspector] public ManagerClasses.GameState gameState;
    private GameManager gameManager = null;
    private ScreenFade screenFade = null;
    private GameObject player = null;
    private PlayerMenuController menuController = null;
    [HideInInspector] public int nextScene = 0;
    [HideInInspector] public bool RingPathIsOn = true;
    public Transform[] spawnPoints = null;
    public void SetupLevelManager(ManagerClasses.GameState s, GameObject p, GameManager g)
    {
        player = p;
        gameState = s;
        gameManager = g;
        menuController = p.GetComponent<PlayerMenuController>();
        screenFade = p.GetComponentInChildren<ScreenFade>();
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