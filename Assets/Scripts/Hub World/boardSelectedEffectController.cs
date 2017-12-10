using UnityEngine;
public class boardSelectedEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] effects = null;
    private void OnEnable()
    {
        EventManager.OnUpdateBoardMenuEffects += SetActiveBoard;
        SetActiveBoard();
    }
    private void OnDisable()
    {
        EventManager.OnUpdateBoardMenuEffects -= SetActiveBoard;
    }
    private void SetActiveBoard()
    {
        for (int i = 0; i < effects.Length; ++i)
            if (i == (int)BoardManager.currentBoardSelection)
                effects[i].Play();
            else
                effects[i].Stop();
    }
}