using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStandSelectBoard : SelectedObject
{
    BoardManager boardManager;

    //our material and board types are stored in the BoardStandScript
    BoardStandProperties selectionVariables;

    Material renderMat = null;

    private void Start()
    {
        boardManager = GameManager.instance.boardScript;

        selectionVariables = GetComponentInParent<BoardStandProperties>();
        renderMat = gameObject.GetComponent<Renderer>().material;
        renderMat.DisableKeyword("_EMISSION");
    }

    public override void selectedFuntion()
    {
        base.selectedFuntion();
        renderMat.EnableKeyword("_EMISSION");
    }
    public override void deSelectedFunction()
    {
        base.deSelectedFunction();
        renderMat.DisableKeyword("_EMISSION");
    }
    override public void selectSuccessFunction()
    {
        //set the player board to one of our pre-defined boards
        boardManager.BoardSelect(selectionVariables.boardType);
        EventManager.OnCallBoardMenuEffects();
    }

}
