using UnityEngine;
public enum GameStates { HubWorld, GamePlay, SceneTransition };
public enum GameModes { Continuous, Cursed, Free, Race, GameModesSize };
public enum GameDifficulties { Easy, Normal, Hard, GameDifficultiesSize };
public static class ManagerClasses
{
    public class RoundTimer
    {
        private float timeLeft = 0.0f, timeInLevel = 0.0f;
        private bool timersPaused = false;
        public RoundTimer(float sTime = 5.0f) { timeLeft = sTime; }
        public void PauseTimer(bool paused) => timersPaused = paused;
        public void IncreaseTimeLeft(float iTime) { if (!timersPaused) timeLeft += iTime; }
        public float TimeLeft { get { return timeLeft; } set { timeLeft = value; } }
        public float TimeInLevel { get { return timeInLevel; } set { timeInLevel = value; } }
        public void UpdateTimers()
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
    public class GameState
    {
        public GameStates currentState = GameStates.HubWorld;
    }
    public class GameMode
    {
        public GameModes currentMode = GameModes.Continuous;
    }
    public class GameDifficulty
    {
        public GameDifficulties currentDifficulty = GameDifficulties.Normal;
    }
    [System.Serializable]
    public class PlayerMovementVariables
    {
        [Header("Speed Values")] public float maxSpeed = 10.0f;
        public float restingSpeed = 5.0f;
        public float minSpeed = 2.0f;
        [Header("Acceleration Values")] public float downwardAcceleration = 30.0f;
        public float restingAcceleration = 30.0f;
        public float upwardAcceleration = 30.0f;
        [Tooltip("The amount to interpolate on every fixed update."), Range(0.001f, 1.0f)] public float momentum = 0.1f;
        [Header("Sensitivities"), Range(0.5f, 5.0f)] public float pitchSensitivity = 3.0f;
        [Range(0.5f, 5f)] public float yawSensitivity = 3.0f;
        [Header("Angles"), Range(10.0f, 75.0f)] public float maxDescendAngle = 30.0f;
        [Tooltip("The threshold at which the object returns to Resting Speed."), Range(0.0f, 20.0f)] public float restingThreshold = 10.0f;
        [Range(10.0f, 75.0f)] public float maxAscendAngle = 30.0f;
        [Header("Rigid Body Values"), Range(0.0f, 10.0f)] public float bounceModifier = 1.0f;
        public float mass = 1.0f;
        public float drag = 1.0f;
        [Tooltip("Angular drag only affects movement after colliding with an object.")] public float angularDrag = 5.0f;
    }
}