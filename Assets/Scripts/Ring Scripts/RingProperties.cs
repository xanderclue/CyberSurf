using UnityEngine;

public class RingProperties : MonoBehaviour
{
    //assign through the inspector
    [SerializeField] private bool duplicatePosition = false;
    public int positionInOrder = 0;
    public float bonusTime = 0.0f;
    public bool lastRingInScene = false;
    public int nextScene = 1;
    public bool DuplicatePosition { get { return duplicatePosition; } }

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
        if (GameModes.Cursed == GameManager.instance.gameMode.currentMode)
        {
            BoardManager boardManager = GameManager.instance.boardScript;
            ManagerClasses.PlayerMovementVariables currPMV = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables, basePMV;
            boardManager.GamepadBoardSelect(out basePMV, BoardType.MachII);
            if (BoardType.MachII != boardManager.currentBoardSelection)
                bonusTime *= (basePMV.minSpeed + basePMV.restingAcceleration + basePMV.maxSpeed) / (currPMV.minSpeed + currPMV.restingSpeed + currPMV.maxSpeed);
        }
    }
}