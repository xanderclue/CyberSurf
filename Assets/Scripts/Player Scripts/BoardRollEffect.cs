using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BoardRollEffect : MonoBehaviour
{
    private Rigidbody playerRB = null;
    private int currScene = 1;
    private float zRotation = 0.0f, prevYRotation = 0.0f, xRotation = 0.0f, forwardSpeed = 0.0f;
    [SerializeField] private PitchRollEffectVariables variables = null;
    [System.Serializable]
    public class PitchRollEffectVariables
    {
        [Header("Roll")] public float rollIncreaseRate = 1.2f;
        public float rollDecreaseRate = 0.1f;
        public float maxRollDegree = 25.0f;
        [Header("Pitch")] public float pitchIncreaseRate = 1.0f;
        public float pitchDecreaseRate = 0.5f;
        public float maxPitchDegree = 20.0f;
    }
    private void LevelSelectionUnlocked(bool locked)
    {
        if (!locked)
        {
            StopAllCoroutines();
            if (null == playerRB)
                playerRB = GetComponentInParent<Rigidbody>();
            zRotation = 0.0f;
            prevYRotation = transform.eulerAngles.y;
            xRotation = 0.0f;
            StartCoroutine(BoardRollCoroutine());
        }
    }
    private void RollEffect()
    {
        if (prevYRotation != transform.eulerAngles.y)
        {
            zRotation += Mathf.DeltaAngle(transform.eulerAngles.y, prevYRotation) * variables.rollIncreaseRate;
            zRotation = Mathf.Clamp(zRotation, -variables.maxRollDegree, variables.maxRollDegree);
            prevYRotation = transform.eulerAngles.y;
        }
        if (0.0f != zRotation)
        {
            zRotation = Mathf.Lerp(zRotation, 0.0f, variables.rollDecreaseRate);
            if (zRotation < 0.1f && zRotation > -0.1f)
                zRotation = 0.0f;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
        }
    }
    private void PitchEffect()
    {
        if (currScene < LevelSelectOptions.LevelBuildOffset)
        {
            forwardSpeed = transform.InverseTransformDirection(playerRB.velocity).z;
            if (forwardSpeed >= 0.1f || forwardSpeed <= -0.1f)
            {
                xRotation += forwardSpeed * variables.pitchIncreaseRate;
                xRotation = Mathf.Clamp(xRotation, -variables.maxPitchDegree, variables.maxPitchDegree);
            }
            if (0.0f != xRotation)
            {
                xRotation = Mathf.Lerp(xRotation, 0.0f, variables.pitchDecreaseRate);
                if (xRotation < 0.1f && xRotation > -0.1f)
                    xRotation = 0.0f;
                transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }
    private IEnumerator BoardRollCoroutine()
    {
        yield return new WaitForFixedUpdate();
        RollEffect();
        PitchEffect();
        StartCoroutine(BoardRollCoroutine());
    }
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        currScene = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        EventManager.OnSelectionLock += LevelSelectionUnlocked;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        EventManager.OnSelectionLock -= LevelSelectionUnlocked;
    }
}