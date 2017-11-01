using UnityEngine;
public class WorldPortalScript : MonoBehaviour
{
    [SerializeField] private bool isDemoMode = false;
    private static readonly System.Type boxCollider = typeof(CapsuleCollider);
    private PlayerMenuController pmc = null;
    private ManagerClasses.GameMode gameMode = null;
    private void Start()
    {
        pmc = GameManager.player.GetComponent<PlayerMenuController>();
        gameMode = GameManager.instance.gameMode;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (boxCollider == other.GetType() && "Board" == other.gameObject.tag)
        {
            if (isDemoMode)
                gameMode.currentMode = GameModes.Continuous;
            int level = GetComponentInParent<WorldPortalProperties>().SceneIndex;
            GameManager gameManager = GameManager.instance;
            gameManager.lastPortalBuildIndex = level;
            EventManager.OnTriggerTransition(level);
            pmc.ToggleMenuMovement(true);
        }
    }
}