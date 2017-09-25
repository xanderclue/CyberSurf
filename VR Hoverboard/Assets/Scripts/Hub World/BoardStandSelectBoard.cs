using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStandSelectBoard : SelectedObject
{
    BoardManager boardManager;

    //our material and board types are stored in the BoardStandScript
    BoardStandProperties selectionVariables;

    private void Start()
    {
        boardManager = GameManager.instance.boardScript;

        selectionVariables = GetComponentInParent<BoardStandProperties>();
    }

    override public void selectSuccessFunction()
    {
        //set the player board to one of our pre-defined boards
        boardManager.BoardSelect(selectionVariables.boardType);   
    }

}
