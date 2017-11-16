using UnityEngine;
using TMPro;
using Xander.Debugging;
using Xander.Dozenal;
public class TopScoreDisplay : MonoBehaviour
{
    private int currentLevel = LevelSelectOptions.LevelBuildOffset;
    [Header("Sink Effect"), SerializeField] private BackBoardSinkEffect sinkEffect = null;
    [SerializeField] private Transform scoreBackBoard = null;
    private bool updateScore = false;
    private ManagerClasses.GameMode gameMode = null;
    [Header("Top Scores"), SerializeField] private TextMeshPro[] highScoreTMPS = null;
    private ScoreManager scoreScript = null;
    private int[] scores = new int[10];
    private float[] times = new float[10];
    private string[] names = new string[10];
    private void Start()
    {
        gameMode = GameManager.instance.gameMode;
        scoreScript = GameManager.instance.scoreScript;
        if (GameManager.instance.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
            currentLevel = GameManager.instance.lastPortalBuildIndex;
        UpdateScoreDisplay();
    }
    public void StartScoreUpdate()
    {
        updateScore = true;
        StartCoroutine(sinkEffect.SinkEffectCoroutine(scoreBackBoard));
    }
    private void UpdateScoreDisplay()
    {
        switch (gameMode.currentMode)
        {
            case GameModes.Continuous:
                for (int i = 0; i < scoreScript.topContinuousScores.Length; ++i)
                {
                    int cumulativeScore = 0;
                    float totalTime = 0.0f;
                    for (int j = 0; j < scoreScript.topContinuousScores[i].levels.Length; ++j)
                    {
                        cumulativeScore += scoreScript.topContinuousScores[i].levels[j].score;
                        totalTime += scoreScript.topContinuousScores[i].levels[j].time;
                    }
                    scores[i] = cumulativeScore;
                    times[i] = totalTime;
                    names[i] = scoreScript.topContinuousScores[i].name;
                }
                break;
            case GameModes.Cursed:
                for (int i = 0; i < scoreScript.topCursedScores[currentLevel].cursedScores.Length; ++i)
                {
                    scores[i] = scoreScript.topCursedScores[currentLevel].cursedScores[i].score;
                    times[i] = scoreScript.topCursedScores[currentLevel].cursedScores[i].time;
                    names[i] = scoreScript.topCursedScores[currentLevel].cursedScores[i].name;
                }
                break;
            case GameModes.Free:
                for (int i = 0; i < 10; ++i)
                {
                    scores[i] = 0;
                    times[i] = 0;
                    names[i] = "NOBODY";
                }
                break;
            case GameModes.Race:
                Debug.Log("To Add race case TopScoreDisplay");
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
        for (int i = 0; i < highScoreTMPS.Length; ++i)
            highScoreTMPS[i].SetText((i + 1).Dozenal() + ": " + names[i] + " | " + scores[i] + " | " + times[i].ToString("n2") + " ");
    }
    private void CheckUpdateFlags()
    {
        if (updateScore)
        {
            UpdateScoreDisplay();
            updateScore = false;
        }
    }
    private void OnEnable()
    {
        BackBoardSinkEffect.StartContentUpdate += CheckUpdateFlags;
    }
    private void OnDisable()
    {
        BackBoardSinkEffect.StartContentUpdate -= CheckUpdateFlags;
    }
}