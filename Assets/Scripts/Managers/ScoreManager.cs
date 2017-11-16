using UnityEngine;
using UnityEngine.SceneManagement;
using Xander.Debugging;
public class ScoreManager : MonoBehaviour
{
    public struct ScoreData
    {
        public Vector3[] positions;
        public Quaternion[] rotations;
        public GameDifficulties difficulty;
        public string name;
        public float time;
        public int score, board;
        public bool isLastScoreInput;
    }
    public struct CursedScores
    {
        public ScoreData[] cursedScores;
        public int currentAmoutFilled;
    }

    public struct RaceScores
    {
        public ScoreData[] racescores;
        public int currentAmoutFilled;
    }
    public struct ContinuousScores
    {
        public ScoreData[] levels;
        public GameDifficulties difficulty;
        public string name;
        public bool isLastScoreInput;
    }
    public CursedScores[] topCursedScores = null;
    public ContinuousScores[] topContinuousScores = null;
    public RaceScores[] topRaceScores = null;
    [HideInInspector] public int curFilledCont = -1, ringHitCount = 0, score = 0;
    [HideInInspector] public bool firstPortal = true, respawnEnabled = true;
    [HideInInspector] public char[] currentName = new char[3];
    public float baseScorePerRing = 100.0f;
    private ManagerClasses.RoundTimer roundTimer = null;
    private ManagerClasses.GameState gameState = null;
    private PlayerRespawn playerRespawnScript = null;
    private Transform[] spawnPoints = null;
    private int respawnCount = 0;
    private const int maxRespawnCount = 3;
    [HideInInspector] public float prevRingBonusTime = 0.0f, score_multiplier = 0.0f;
    [HideInInspector] public Transform prevRingTransform = null;
    private void InitScores()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        topCursedScores = SaveLoader.LoadCurseScores();
        topRaceScores = SaveLoader.LoadRaceScores();
        topContinuousScores = SaveLoader.LoadContinuousScores();
        if (null == topCursedScores)
        {
            topCursedScores = new CursedScores[sceneCount];
            for (int i = 0; i < sceneCount; ++i)
            {
                topCursedScores[i].cursedScores = new ScoreData[10];
                topCursedScores[i].currentAmoutFilled = 0;
            }
        }
        if (null == topContinuousScores)
        {
            topContinuousScores = new ContinuousScores[10];
            for (int i = 0; i < 10; ++i)
                topContinuousScores[i].levels = new ScoreData[sceneCount];
        }
    }
    public void SetupScoreManager()
    {
        gameState = GameManager.instance.gameState;
        spawnPoints = GameManager.instance.levelScript.spawnPoints;
        playerRespawnScript = GameManager.player.GetComponent<PlayerRespawn>();
        roundTimer = GameManager.instance.roundTimer;
        score = respawnCount = 0;
        InitScores();
    }
    public void LevelEnd()
    {
        positionRecorder recorder = GameManager.player.GetComponent<positionRecorder>();
        int level = SceneManager.GetActiveScene().buildIndex;
        ScoreData newLevelScore = new ScoreData();
        switch (GameManager.instance.gameMode.currentMode)
        {
            case GameModes.Continuous:
                if (firstPortal)
                {
                    ++curFilledCont;
                    firstPortal = false;
                }
                newLevelScore.score = score;
                newLevelScore.time = roundTimer.TimeInLevel;
                newLevelScore.board = (int)GameManager.instance.boardScript.currentBoardSelection;
                newLevelScore.positions = recorder.positions.ToArray();
                newLevelScore.rotations = recorder.rotations.ToArray();
                if (curFilledCont < 10)
                {
                    topContinuousScores[curFilledCont].difficulty = GameManager.instance.gameDifficulty.currentDifficulty;
                    topContinuousScores[curFilledCont].levels[level] = newLevelScore;
                    topContinuousScores[curFilledCont].isLastScoreInput = true;
                }
                else
                {
                    topContinuousScores[9].difficulty = GameManager.instance.gameDifficulty.currentDifficulty;
                    topContinuousScores[9].levels[level] = newLevelScore;
                    topContinuousScores[9].isLastScoreInput = true;
                }
                SortContinuousScores(topContinuousScores);
                break;
            case GameModes.Cursed:
                newLevelScore.score = score;
                newLevelScore.time = roundTimer.TimeLeft;
                newLevelScore.board = (int)GameManager.instance.boardScript.currentBoardSelection;
                newLevelScore.positions = recorder.positions.ToArray();
                newLevelScore.rotations = recorder.rotations.ToArray();
                newLevelScore.isLastScoreInput = true;
                newLevelScore.difficulty = GameManager.instance.gameDifficulty.currentDifficulty;
                if (topCursedScores[level].currentAmoutFilled < 10)
                {
                    topCursedScores[level].cursedScores[topCursedScores[level].currentAmoutFilled] = newLevelScore;
                    ++topCursedScores[level].currentAmoutFilled;
                }
                else
                    topCursedScores[level].cursedScores[9] = newLevelScore;
                SortCurseScores(topCursedScores[level].cursedScores, topCursedScores[level].currentAmoutFilled);
                break;
            case GameModes.Free:
                break;
            case GameModes.Race:
                newLevelScore.score = score;
                newLevelScore.time = roundTimer.TimeLeft;
                newLevelScore.board = (int)GameManager.instance.boardScript.currentBoardSelection;
                newLevelScore.positions = recorder.positions.ToArray();
                newLevelScore.rotations = recorder.rotations.ToArray();
                newLevelScore.isLastScoreInput = true;
                newLevelScore.difficulty = GameManager.instance.gameDifficulty.currentDifficulty;
                if (topCursedScores[level].currentAmoutFilled < 10)
                {
                    topCursedScores[level].cursedScores[topCursedScores[level].currentAmoutFilled] = newLevelScore;
                    ++topCursedScores[level].currentAmoutFilled;
                }
                else
                    topCursedScores[level].cursedScores[9] = newLevelScore;
                SortCurseScores(topCursedScores[level].cursedScores, topCursedScores[level].currentAmoutFilled);
                break;
            default:
                Debug.LogWarning("Missing case: \"" + GameManager.instance.gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
    }
    private void SortCurseScores(ScoreData[] scores, int arrayLength)
    {
        int curr = 1, comparer;
        ScoreData storedScore;
        while (curr < arrayLength)
        {
            storedScore = scores[curr];
            comparer = curr - 1;
            while (comparer >= 0)
            {
                if (scores[comparer].score < storedScore.score)
                {
                    scores[comparer + 1] = scores[comparer];
                    --comparer;
                }
                else if (scores[comparer].score == storedScore.score && scores[comparer].time > storedScore.time)
                {
                    scores[comparer + 1] = scores[comparer];
                    --comparer;
                }
                else
                    break;
            }
            scores[comparer + 1] = storedScore;
            ++curr;
        }
    }
    private void SortContinuousScores(ContinuousScores[] array)
    {
        int i, j, k, keyScore, cumulativeScore;
        ContinuousScores key;
        for (i = 1; i < array.Length; ++i)
        {
            key = array[i];
            keyScore = 0;
            for (j = 0; j < array[i].levels.Length; ++j)
                keyScore += array[i].levels[j].score;
            j = i - 1;
            while (j >= 0)
            {
                cumulativeScore = 0;
                for (k = 0; k < array[j].levels.Length; ++k)
                    cumulativeScore += array[j].levels[k].score;
                if (cumulativeScore < keyScore)
                {
                    array[j + 1] = array[j];
                    --j;
                }
                else
                    break;
            }
            array[j + 1] = key;
        }
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        prevRingTransform = spawnPoints[SceneManager.GetActiveScene().buildIndex];
        roundTimer.TimeInLevel = 0.0f;
        prevRingBonusTime = 0.0f;
        respawnCount = 0;
        respawnEnabled = GameModes.Cursed == GameManager.instance.gameMode.currentMode;
    }
    private void Update()
    {
        if (GameStates.GamePlay == gameState.currentState)
        {
            roundTimer.UpdateTimers();
            if (respawnEnabled && roundTimer.TimeLeft <= 0.0 && !playerRespawnScript.IsRespawning)
            {
                if (respawnCount < maxRespawnCount)
                    playerRespawnScript.RespawnPlayer(prevRingTransform, 5.0f + prevRingBonusTime);
                else
                    EventManager.OnTriggerTransition(1);
                ++respawnCount;
            }
        }
    }
    private void OnEnable() => SceneManager.sceneLoaded += OnLevelLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnLevelLoaded;
    private void OnApplicationQuit() => SaveLoader.Save();
}