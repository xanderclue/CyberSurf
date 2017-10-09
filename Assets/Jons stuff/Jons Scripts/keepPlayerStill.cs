using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepPlayerStill : MonoBehaviour
{
    public static bool tutorialOn;
    // Use this for initialization
    Rigidbody player;
    void Start()
    {
        tutorialOn = true;
        player = gameObject.GetComponent<Rigidbody>();
      //  GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(player.gameObject.transform.position);

    }


    void Update()
    {
        if (tutorialOn == true)
            GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(player.gameObject.transform.position ,player.gameObject.transform.rotation);
        else
            GameManager.player.GetComponent<PlayerMenuController>().UnlockPlayerPosition();



    }
}
