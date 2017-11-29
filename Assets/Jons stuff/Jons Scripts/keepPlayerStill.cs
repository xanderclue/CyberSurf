using UnityEngine;
public class keepPlayerStill : MonoBehaviour
{
    public static bool tutorialOn;
    private void Start()
    {
        tutorialOn = true;
        Rigidbody playerRB = GetComponent<Rigidbody>();
        GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(playerRB.transform.position, playerRB.transform.rotation);
    }
    private void Update()
    {
        if (!tutorialOn)
        {
        
            GameManager.player.GetComponent<PlayerMenuController>().UnlockPlayerPosition();
            Destroy(this);
        }
    }
}