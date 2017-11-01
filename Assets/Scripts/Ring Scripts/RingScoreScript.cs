using UnityEngine;
public class RingScoreScript : MonoBehaviour
{
    [System.Serializable]
    private class ScoreMultipliers
    { public float speedMultiplier = 1.5f, consecutiveMultiplier = 1.0f, consecutiveIncreaseAmount = 0.25f; }
    private static readonly System.Type capsuleType = typeof(CapsuleCollider);
    private static ScoreMultipliers multipliers = new ScoreMultipliers();
    private static int prevPositionInOrder = -1, prevConsecutiveCount = 0, consecutiveCount = 0;
    private static bool effectsStopped = true;
    private playerCollisionSoundEffects pColSoundEffects = null;
    private PlayerArrowHandler pArrowHandler = null;
    private ScoreManager scoreManager = null;
    private RingProperties rp = null;
    private float originalCrInAmt = 0.25f;
    private bonusTimeTextUpdater bonusTimeText = null;
    private ParticleSystem hitEffect = null;
    private PlayerRespawn respawnScript = null;
    public GameObject portaleffect;
    private void Start()
    {
        scoreManager = GameManager.instance.scoreScript;
        pColSoundEffects = GameManager.player.GetComponent<playerCollisionSoundEffects>();
        pArrowHandler = GameManager.player.GetComponent<PlayerArrowHandler>();
        rp = GetComponent<RingProperties>();
        bonusTimeText = GameManager.player.GetComponentsInChildren<bonusTimeTextUpdater>(true)[0];
        hitEffect = GetComponentInChildren<ParticleSystem>();
        respawnScript = GameManager.player.GetComponent<PlayerRespawn>();
        prevPositionInOrder = -1;
        consecutiveCount = 0;
        effectsStopped = true;
        originalCrInAmt = multipliers.consecutiveIncreaseAmount;
        if (GameModes.Race == GameManager.instance.gameMode.currentMode && rp.lastRingInScene == true)
        {
            portaleffect.SetActive(false);
        }
    }
    private void IncreaseScore()
    {
        float totalMultiplier = 1.0f;
        if (prevPositionInOrder + 1 == rp.positionInOrder)
        {
            totalMultiplier += multipliers.consecutiveMultiplier;
            multipliers.consecutiveMultiplier += multipliers.consecutiveIncreaseAmount;
            ++consecutiveCount;
        }
        else
        {
            multipliers.consecutiveMultiplier = originalCrInAmt;
            consecutiveCount = prevConsecutiveCount = 0;
            if (!effectsStopped)
            {
                effectsStopped = true;
                EventManager.StopRingPulse();
            }
        }
        scoreManager.score += (int)(scoreManager.baseScorePerRing * totalMultiplier);
        scoreManager.score_multiplier = totalMultiplier;
    }
    private void UpdateRingEffects()
    {
        if (consecutiveCount > prevConsecutiveCount)
        {
            if (effectsStopped && 3 == consecutiveCount)
            {
                effectsStopped = false;
                EventManager.StartRingPulse();
            }
            prevConsecutiveCount = consecutiveCount;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!respawnScript.IsRespawning && capsuleType == other.GetType() && "Player" == other.tag)
        {
            pArrowHandler.UpdatePlayerHUDPointer(rp);

            if (rp.positionInOrder > prevPositionInOrder)
            {
                scoreManager.prevRingBonusTime = rp.bonusTime;
                scoreManager.prevRingTransform = rp.transform;
                ++scoreManager.ringHitCount;
                if (GameModes.Cursed == GameManager.instance.gameMode.currentMode)
                {
                    GameManager.instance.roundTimer.IncreaseTimeLeft(rp.bonusTime);
                    bonusTimeText.play((rp.bonusTime).ToString("n2"));
                }
                IncreaseScore();
                UpdateRingEffects();
                pColSoundEffects.PlayRingClip(gameObject);
                if (null != hitEffect)
                {
                    hitEffect.Play();
                    hitEffect.GetComponentInParent<MeshRenderer>().enabled = false;
                }
                prevPositionInOrder = rp.positionInOrder;
            }
            if (rp.lastRingInScene)
            {
                if (GameModes.Race != GameManager.instance.gameMode.currentMode)
                {
                    scoreManager.LevelEnd();
                    scoreManager.prevRingBonusTime = 0.0f;
                    scoreManager.prevRingTransform = GameManager.instance.levelScript.spawnPoints[rp.nextScene];
                    scoreManager.ringHitCount = 0;
                    prevPositionInOrder = -1;
                    EventManager.OnTriggerTransition(rp.nextScene);
                }
                else
                {
                    if (rp.laps == 1)
                    {
                        scoreManager.LevelEnd();
                        scoreManager.prevRingBonusTime = 0.0f;
                        scoreManager.prevRingTransform = GameManager.instance.levelScript.spawnPoints[rp.nextScene];
                        scoreManager.ringHitCount = 0;
                        prevPositionInOrder = -1;
                        EventManager.OnTriggerTransition(rp.nextScene);
                    }
                    else
                    {
                        rp.laps += 1;
                        if (rp.laps == 1)
                        {
                            portaleffect.SetActive(true);
                        }
                    }
                }
                
            }
        }
    }
}