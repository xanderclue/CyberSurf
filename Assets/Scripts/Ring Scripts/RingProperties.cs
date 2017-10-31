using UnityEngine;
public class RingProperties : MonoBehaviour
{
    [SerializeField] private bool duplicatePosition = false;
    public int positionInOrder = 0;
    public float bonusTime = 0.0f;
    public bool lastRingInScene = false;
    public int nextScene = 1;
    public int laps = 0;
    public bool DuplicatePosition { get { return duplicatePosition; } }
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
            ManagerClasses.PlayerMovementVariables
                currPMV = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables,
                basePMV = boardManager.GamepadBoardSelect(BoardType.MachII);
            if (BoardType.MachII != boardManager.currentBoardSelection)
                bonusTime *= (basePMV.minSpeed + basePMV.restingAcceleration + basePMV.maxSpeed) / (currPMV.minSpeed + currPMV.restingSpeed + currPMV.maxSpeed);
        }
        else if (GameModes.Race == GameManager.instance.gameMode.currentMode)
        {
            if (nextScene != 1)
            {
                gameObject.SetActive(false);
            }
    }
    }
}