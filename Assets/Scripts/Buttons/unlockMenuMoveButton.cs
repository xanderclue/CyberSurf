using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockMenuMoveButton : SelectedObject
{
    public GameObject panel;

    override public void SuccessFunction()
    {
        GameManager.player.GetComponent<PlayerMenuController>().ToggleMenuMovement(false);
        gameObject.SetActive(false);
        panel.SetActive(false);
    }
}
