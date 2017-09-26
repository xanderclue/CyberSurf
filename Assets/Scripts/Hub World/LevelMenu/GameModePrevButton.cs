using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModePrevButton : SelectedObject
{
    LevelMenu levelMenu;

    private void Start()
    {
        levelMenu = GetComponentInParent<LevelMenu>();
    }

    public override void selectSuccessFunction()
    {
        levelMenu.PreviousGameMode();
    }

}
