﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficultyNextButton : SelectedObject
{
    LevelMenu levelMenu;

    new private void Start()
    {
        base.Start();
        levelMenu = GetComponentInParent<LevelMenu>();
    }

    public override void SuccessFunction()
    {
        levelMenu.NextDifficulty();
    }

}
