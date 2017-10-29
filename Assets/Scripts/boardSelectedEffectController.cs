using UnityEngine;
public class boardSelectedEffectController : MonoBehaviour
{
    private BoardStandProperties[] boardEffects = null;
    private BoardManager boardManager = null;
    private void Start()
    {
        boardEffects = GetComponentsInChildren<BoardStandProperties>();
        boardManager = GameManager.instance.boardScript;
        EventManager.OnCallBoardMenuEffects();
    }
    private void OnEnable()
    {
        EventManager.OnUpdateBoardMenuEffects += SetActiveBoard;
    }
    private void OnDisable()
    {
        EventManager.OnUpdateBoardMenuEffects -= SetActiveBoard;
    }
    private void SetActiveBoard()
    {
        foreach (BoardStandProperties boardEffect in boardEffects)
            if (boardEffect.boardType == boardManager.currentBoardSelection)
                boardEffect.GetComponentInChildren<ParticleSystem>().Play();
            else
                boardEffect.GetComponentInChildren<ParticleSystem>().Stop();
    }
}