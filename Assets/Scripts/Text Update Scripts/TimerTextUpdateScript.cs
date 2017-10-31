using UnityEngine;
using TMPro;
using Xander.Debugging;
public class TimerTextUpdateScript : MonoBehaviour
{
    private ManagerClasses.RoundTimer roundTimer = null;
    private TextMeshProUGUI element = null;
    private bool textIsRed = false;
    private float timeToTurnTextRed = 2.0f;
    private Color originalTextColor;
    private GameManager gameManager = null;
    private string textToWrite = "TIMER BROKE";
    private void Start()
    {
        gameManager = GameManager.instance;
        roundTimer = gameManager.roundTimer;
        element = GetComponent<TextMeshProUGUI>();
        textIsRed = false;
        timeToTurnTextRed = 2.0f;
        originalTextColor = element.color;
    }
    private void Update()
    {
        switch (gameManager.gameMode.currentMode)
        {
            case GameModes.Continuous:
                textToWrite = " " + roundTimer.TimeInLevel.ToString("n2") + " ";
                break;
            case GameModes.Cursed:
                if (!textIsRed && roundTimer.TimeLeft < timeToTurnTextRed)
                {
                    element.color = Color.red;
                    textIsRed = true;
                }
                else if (textIsRed && roundTimer.TimeLeft > timeToTurnTextRed)
                {
                    element.color = originalTextColor;
                    textIsRed = false;
                }
                textToWrite = " " + roundTimer.TimeLeft.ToString("n2") + " ";
                break;
            case GameModes.Free:
                textToWrite = " " + roundTimer.TimeInLevel.ToString("n2") + " ";
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameManager.gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                textToWrite = "TIMER BROKE";
                break;
        }
        element.SetText(textToWrite);
    }
}