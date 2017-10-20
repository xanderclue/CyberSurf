using UnityEngine;

public class keepPlayerStill : MonoBehaviour
{
    public static bool tutorialOn;
    void Start()
    {
        tutorialOn = true;
        Rigidbody player = gameObject.GetComponent<Rigidbody>();
        GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(player.gameObject.transform.position, player.gameObject.transform.rotation);
    }

    void Update()
    {
        if (!tutorialOn)
        {
            GameManager.player.GetComponent<PlayerMenuController>().UnlockPlayerPosition();
            enabled = false;
        }
    }
}