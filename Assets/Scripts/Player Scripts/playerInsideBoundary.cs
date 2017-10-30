using UnityEngine;
public class playerInsideBoundary : MonoBehaviour
{
    private PlayerRespawn playerRespawnScript = null;
    private ScoreManager scoreScript = null;
    private void Start()
    {
        playerRespawnScript = GetComponent<PlayerRespawn>();
        scoreScript = GameManager.instance.scoreScript;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ("Boundary" == collision.gameObject.tag)
            playerRespawnScript.RespawnPlayer(scoreScript.prevRingTransform, 5.0f + scoreScript.prevRingBonusTime);
    }
}