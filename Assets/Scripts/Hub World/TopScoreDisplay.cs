using UnityEngine;
using TMPro;
using Xander.Dozenal;
public class TopScoreDisplay : MonoBehaviour
{
    private int currentLevel = LevelSelectOptions.LevelBuildOffset;
    [Header("Sink Effect"), SerializeField] private BackBoardSinkEffect sinkEffect = null;
    [SerializeField] private Transform scoreBackBoard = null;
    private bool updateScore = false;
    [Header("Top Scores"), SerializeField] private TextMeshPro[] highScoreTMPS = null;
    private int[] scores = new int[10];
    private float[] times = new float[10];
    private string[] names = new string[10];
    private void Start()
    {
        if (GameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
            currentLevel = GameManager.lastPortalBuildIndex;
        UpdateScoreDisplay();
    }
    public void StartScoreUpdate()
    {
        updateScore = true;
        StartCoroutine(sinkEffect.SinkEffectCoroutine(scoreBackBoard));
    }
    private void UpdateScoreDisplay()
    {
        switch (GameManager.gameMode)
        {
            case GameMode.Continuous:
                for (int i = 0; i < ScoreManager.topContinuousScores.Length; ++i)
                {
                    int cumulativeScore = 0;
                    float totalTime = 0.0f;
                    for (int j = 0; j < ScoreManager.topContinuousScores[i].levels.Length; ++j)
                    {
                        cumulativeScore += ScoreManager.topContinuousScores[i].levels[j].score;
                        totalTime += ScoreManager.topContinuousScores[i].levels[j].time;
                    }
                    scores[i] = cumulativeScore;
                    times[i] = totalTime;
                    names[i] = ScoreManager.topContinuousScores[i].name;
                }
                break;
            case GameMode.Cursed:
                for (int i = 0; i < ScoreManager.topCursedScores[currentLevel].cursedScores.Length; ++i)
                {
                    scores[i] = ScoreManager.topCursedScores[currentLevel].cursedScores[i].score;
                    times[i] = ScoreManager.topCursedScores[currentLevel].cursedScores[i].time;
                    names[i] = ScoreManager.topCursedScores[currentLevel].cursedScores[i].name;
                }
                break;
            case GameMode.Free:
                for (int i = 0; i < 10; ++i)
                {
                    scores[i] = 0;
                    times[i] = 0;
                    names[i] = "NOBODY";
                }
                break;
            case GameMode.Race:
                for (int i = 0; i < ScoreManager.topRaceScores[currentLevel].racescores.Length; ++i)
                {
                    scores[i] = ScoreManager.topRaceScores[currentLevel].racescores[i].score;
                    times[i] = ScoreManager.topRaceScores[currentLevel].racescores[i].time;
                    names[i] = ScoreManager.topRaceScores[currentLevel].racescores[i].name;
                }
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