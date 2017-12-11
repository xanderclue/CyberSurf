using UnityEngine;
public static class RoundTimer
{
    public static float timeLeft = 5.0f, timeInLevel = 0.0f;
    public static bool timersPaused = false;
    public static void IncreaseTimeLeft(float iTime) { if (!timersPaused) timeLeft += iTime; }
    public static void UpdateTimers()
    {
        if (!timersPaused)
        {
            if (timeLeft - Time.deltaTime > 0.0f)
                timeLeft -= Time.deltaTime;
            else
                timeLeft = 0.0f;
            timeInLevel += Time.deltaTime;
        }
    }
}