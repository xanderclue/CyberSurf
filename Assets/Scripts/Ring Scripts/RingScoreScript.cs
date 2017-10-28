using UnityEngine;

public class RingScoreScript : MonoBehaviour
{
    [System.Serializable]
    public class ScoreMultipliers
    {
        public float speedMultiplier;
        public float consecutiveMultiplier;
        public float consecutiveIncreaseAmount;

        public ScoreMultipliers(float sMul, float crMul, float crInAmt) { speedMultiplier = sMul; consecutiveMultiplier = crMul; consecutiveIncreaseAmount = crInAmt; }
    }

    //static ring effect settings
    public static ScoreMultipliers multipliers = new ScoreMultipliers(1.5f, 1f, 0.25f);
    static int prevPositionInOrder;
    static int prevConsecutiveCount;
    static int consecutiveCount;
    static bool effectsStopped;

    playerCollisionSoundEffects pColSoundEffects;
    PlayerArrowHandler pArrowHandler;
    ScoreManager scoreManager;

    RingProperties rp;
    private static readonly System.Type capsuleType = typeof(CapsuleCollider);

    float originalCrInAmt;

    bonusTimeTextUpdater bonusTimeText;
    ParticleSystem hitEffect;

    PlayerRespawn respawnScript;

    private void Start()
    {
        scoreManager = GameManager.instance.scoreScript;
        pColSoundEffects = GameManager.player.GetComponent<playerCollisionSoundEffects>();
        pArrowHandler = GameManager.player.GetComponent<PlayerArrowHandler>();
        rp = GetComponent<RingProperties>();
        bonusTimeText = GameManager.player.GetComponentsInChildren<bonusTimeTextUpdater>(true)[0];
        hitEffect = GetComponentInChildren<ParticleSystem>();
        respawnScript = GameManager.player.GetComponent<PlayerRespawn>();

        //set our prevRingInOrder to -1, so we don't apply a consecutive score multiplier for the very first ring we go through in a scene
        prevPositionInOrder = -1;
        consecutiveCount = 0;
        effectsStopped = true;

        originalCrInAmt = multipliers.consecutiveIncreaseAmount;
    }

    void IncreaseScore()
    {
        float totalMultiplier = 1.0f;

        if (prevPositionInOrder + 1 == rp.positionInOrder)
        {
            //use our consecutive multiplier if this ring comes immediately after the previous one
            totalMultiplier += multipliers.consecutiveMultiplier;

            //then increase by our increase amount
            multipliers.consecutiveMultiplier += multipliers.consecutiveIncreaseAmount;

            ++consecutiveCount;
        }
        else
        {
            //otherwise, reset the consecutiveMultiplier
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
        //print("Score is now: " + scoreManager.score);
    }

    void UpdateRingEffects()
    {
        if (consecutiveCount > prevConsecutiveCount)
        {
            switch (consecutiveCount)
            {
                case 3:
                    if (effectsStopped)
                    {
                        effectsStopped = false;
                        EventManager.StartRingPulse();
                    }
                    break;
                case 5:
                    break;
                case 10:
                    break;
                default:
                    break;
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
                //update our scoreManager values
                scoreManager.prevRingBonusTime = rp.bonusTime;
                scoreManager.prevRingTransform = rp.transform;
                ++scoreManager.ringHitCount;

                if (GameManager.instance.gameMode.currentMode == GameModes.Cursed)
                {
                    GameManager.instance.roundTimer.IncreaseTimeLeft(rp.bonusTime);
                    bonusTimeText.play((rp.bonusTime).ToString("n2"));
                }

                IncreaseScore();
                UpdateRingEffects();
                pColSoundEffects.PlayRingClip(gameObject);

                if (null != hitEffect)
                {
                    //Debug.Log("Hit a Ring");
                    hitEffect.Play();
                    //MeshRenderer tmp = hitEffect.GetComponentInParent<MeshRenderer>();
                    hitEffect.GetComponentInParent<MeshRenderer>().gameObject.GetComponent<Renderer>().enabled = false;
                }
                else
                    print("HIT EFFECT NULL");

                prevPositionInOrder = rp.positionInOrder;
            }

            if (rp.lastRingInScene)
            {
                scoreManager.levelEnd();

                //update our scoreManager values
                scoreManager.prevRingBonusTime = 0f;
                scoreManager.prevRingTransform = GameManager.instance.levelScript.spawnPoints[rp.nextScene];
                scoreManager.ringHitCount = 0;

                prevPositionInOrder = -1;

                EventManager.OnTriggerTransition(rp.nextScene);
            }
        }
    }
}
