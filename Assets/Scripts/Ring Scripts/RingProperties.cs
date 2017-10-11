using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingProperties : MonoBehaviour
{
    //assign through the inspector
    public bool duplicatePosition = false;
    public int positionInOrder = 0;
    public float bonusTime = 0.0f;
    public bool lastRingInScene = false;
    public int nextScene = 1;

    //bonus time
    BoardManager boardManager;
    ManagerClasses.PlayerMovementVariables currPMV;
    ManagerClasses.PlayerMovementVariables basePMV;

    //if we have children, set their values, but only if this is marked as a duplicate
    private void Awake()
    {
        if (duplicatePosition)
        {
            RingProperties[] rps = GetComponentsInChildren<RingProperties>();

            foreach (RingProperties rp in rps)
            {
                rp.duplicatePosition = duplicatePosition;
                rp.positionInOrder = positionInOrder;
                rp.bonusTime = bonusTime;
                rp.lastRingInScene = lastRingInScene;
                rp.nextScene = nextScene;
            }
        }
    }

    private void Start()
    {
        if (GameManager.instance.gameMode.currentMode == GameModes.Cursed)
        {
            //bonus time stuff
            boardManager = GameManager.instance.boardScript;
            currPMV = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables;
            boardManager.GamepadBoardSelect(out basePMV, BoardType.MachII);

            //calculate averages
            float currentAverage = (currPMV.minSpeed + currPMV.restingSpeed + currPMV.maxSpeed) / 3f;
            float baseAverage;
            float percentDifference;
            float percentIncrease = 1.1f;

            //set our baseAverage
            if (boardManager.currentBoardSelection != BoardType.MachII)
                baseAverage = (basePMV.minSpeed + basePMV.restingAcceleration + basePMV.maxSpeed) / 3f;
            else
                baseAverage = currentAverage;

            //calculate the percent difference depending on how much over/under we are based off of the mach II board average velocity
            percentDifference = (baseAverage / currentAverage) * percentIncrease;

            //increase/decrease the bonus time based off of our percentDifference
            bonusTime *= percentDifference;
        }
    }

}
