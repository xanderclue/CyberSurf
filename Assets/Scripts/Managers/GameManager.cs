using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;
using UnityEngine.PostProcessing;

//our ManagerLoader prefab will ensure that an instance of GameManager is loaded
public class GameManager : MonoBehaviour
{
    //store our game game state
    public ManagerClasses.GameState gameState = new ManagerClasses.GameState();

    //store our game mode
    public ManagerClasses.GameMode gameMode = new ManagerClasses.GameMode();

    //store our game difficulty
    public ManagerClasses.GameDifficulty gameDifficulty = new ManagerClasses.GameDifficulty();

    //store our round timer
    public ManagerClasses.RoundTimer roundTimer = new ManagerClasses.RoundTimer();

    //last level and mode we were in
    [HideInInspector] public GameModes lastMode;
    [HideInInspector] public int lastPortalBuildIndex = 0;
    [HideInInspector] public int lastBuildIndex = -1;

    //set our player prefab through the inspector
    [SerializeField] GameObject playerPrefab;

    //variable for singleton, static makes this variable the same through all GameManager objects
    public static GameManager instance = null;

    //variable to store our player clone
    public static GameObject player = null;

    //store our managers
    [HideInInspector] public ScoreManager scoreScript;
    [HideInInspector] public LevelManager levelScript;
    [HideInInspector] public BoardManager boardScript;
    [HideInInspector] public KeyInputManager keyInputScript;

    //for recording purposes
    [SerializeField] bool disableVignette = false;

    void Awake()
    {
        //make sure we only have one instance of GameManager
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //ensures that our game manager persists between scenes
        DontDestroyOnLoad(gameObject);

        //store our managers
        scoreScript = GetComponent<ScoreManager>();
        levelScript = GetComponent<LevelManager>();
        boardScript = GetComponent<BoardManager>();
        keyInputScript = GetComponent<KeyInputManager>();

        //Instantiate our player, store the clone, then make sure it persists between scenes
        player = Instantiate(playerPrefab);
        DontDestroyOnLoad(player);

        //enable/disable vignette depending on if we are using a HMD
        if (VRDevice.isPresent && !disableVignette)
            player.GetComponentInChildren<PostProcessingBehaviour>().profile.vignette.enabled = true;
        else
            player.GetComponentInChildren<PostProcessingBehaviour>().profile.vignette.enabled = false;

        //set the game to run in the background
        Application.runInBackground = true;

        //set the game difficulty depending on if a HMD is present
        if (VRDevice.isPresent)
            gameDifficulty.currentDifficulty = GameDifficulties.Easy;

        InitGame();
    }

    private void OnEnable()
    {
        GameSettings.GetBool("RingPath", ref levelScript.RingPathIsOn);
        GameSettings.GetBool("Respawn", ref scoreScript.respawnEnabled);
        GameSettings.GetBool("DebugSpeed", ref boardScript.debugSpeedEnabled);
    }
    private void OnDisable()
    {
        GameSettings.SetBool("RingPath", levelScript.RingPathIsOn);
        GameSettings.SetBool("Respawn", scoreScript.respawnEnabled);
        GameSettings.SetBool("DebugSpeed", boardScript.debugSpeedEnabled);
        GameSettings.SetInt("GameDifficulty", unchecked((int)gameDifficulty.currentDifficulty));
        GameSettings.SetInt("GameMode", unchecked((int)gameMode.currentMode));
    }

    private void OnDestroy() { GameSettings.Save(); }

    //using this instead of Awake() in our scripts allows us to control the execution order
    void InitGame()
    {
        gameDifficulty.currentDifficulty = unchecked((GameDifficulties)GameSettings.GetInt("GameDifficulty", unchecked((int)gameDifficulty.currentDifficulty)));
        gameMode.currentMode = unchecked((GameModes)GameSettings.GetInt("GameMode", unchecked((int)gameMode.currentMode)));
        boardScript.SetupBoardManager(player);
        levelScript.SetupLevelManager(gameState, player, instance);
        scoreScript.SetupScoreManager();
        keyInputScript.setupKeyInputManager(gameState);
    }
}