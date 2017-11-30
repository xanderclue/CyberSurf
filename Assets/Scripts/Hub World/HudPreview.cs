using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HudPreview : MonoBehaviour
{
    [HideInInspector] public bool reticleOnOff = false;
    [HideInInspector] public bool speedOnOff = false;
    [HideInInspector] public bool timerOnOff = false;
    [HideInInspector] public bool scoreOnOff = false;
    [HideInInspector] public bool playersOnOff = false;
    [HideInInspector] public bool compassOnOff = false;
    [HideInInspector] public bool arrowOnOff = false;
    [HideInInspector] public bool lapCounterOnOff = false;
    [HideInInspector] public bool positionOnOff = false;
    [HideInInspector] public Color color = Color.clear;
    [SerializeField] private TextMeshProUGUI timer = null, bestLapTime = null, currentLapTime = null, checkpointTime = null, timeGap = null, score = null, scoreMultiplier = null, position = null, lap = null, playerList = null, speed = null;
    [SerializeField] private TextMeshPro northLetter = null;
    [SerializeField] private Image compassBackground = null, handle = null, speedBar = null, reticle = null, reticleBackground = null;
    public void UpdatePreview()
    {
        timer.color = color;
        bestLapTime.color = color;
        currentLapTime.color = color;
        checkpointTime.color = color;
        timeGap.color = color;
        score.color = color;
        scoreMultiplier.color = color;
        position.color = color;
        lap.color = color;
        playerList.color = color;
        speed.color = color;
        northLetter.color = color;
        compassBackground.color = color;
        handle.color = color;
        speedBar.color = color;
        {
            Color reticleColor = color;
            reticleColor.a = reticleBackground.color.a;
            reticleBackground.color = reticleColor;
            reticleColor.a = reticle.color.a;
            reticle.color = reticleColor;
        }
        timer.gameObject.SetActive(timerOnOff);
        bestLapTime.gameObject.SetActive(timerOnOff);
        currentLapTime.gameObject.SetActive(timerOnOff);
        checkpointTime.gameObject.SetActive(timerOnOff);
        timeGap.gameObject.SetActive(timerOnOff);
        score.gameObject.SetActive(scoreOnOff);
        scoreMultiplier.gameObject.SetActive(scoreOnOff);
        position.gameObject.SetActive(positionOnOff);
        lap.gameObject.SetActive(lapCounterOnOff);
        playerList.gameObject.SetActive(playersOnOff);
        speed.gameObject.SetActive(speedOnOff);
        northLetter.gameObject.SetActive(compassOnOff);
        compassBackground.gameObject.SetActive(arrowOnOff);
        handle.gameObject.SetActive(arrowOnOff);
        speedBar.gameObject.SetActive(speedOnOff);
        reticleBackground.gameObject.SetActive(reticleOnOff);
        reticle.gameObject.SetActive(reticleOnOff);
    }
}