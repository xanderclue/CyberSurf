using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStandSelectBoard : SelectedObject
{
    BoardManager boardManager;

    //our material and board types are stored in the BoardStandScript
    BoardStandProperties selectionVariables;

    Material renderer = null;
    bool isPlaying = false;

    private void Start()
    {
        boardManager = GameManager.instance.boardScript;

        selectionVariables = GetComponentInParent<BoardStandProperties>();
        renderer = gameObject.GetComponent<Renderer>().material;
        renderer.DisableKeyword("_EMISSION");
    }

    public override void selectedFuntion()
    {
        renderer.EnableKeyword("_EMISSION");
    }
    public override void deSelectedFunction()
    {
        renderer.DisableKeyword("_EMISSION");
    }
    override public void selectSuccessFunction()
    {
        //set the player board to one of our pre-defined boards
        boardManager.BoardSelect(selectionVariables.boardType);
        EventManager.OnCallBoardMenuEffects();
    }

}
