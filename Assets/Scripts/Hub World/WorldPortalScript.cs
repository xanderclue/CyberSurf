using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldPortalScript : MonoBehaviour
{
    [SerializeField] bool isDemoMode = false;

    System.Type boxCollider;
    PlayerMenuController pmc;

    ManagerClasses.GameMode gameMode;

    private void Start()
    {
        boxCollider = typeof(UnityEngine.CapsuleCollider);
        pmc = GameManager.player.GetComponent<PlayerMenuController>();
        gameMode = GameManager.instance.gameMode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == boxCollider && other.gameObject.tag == "Board")
        {
            if (isDemoMode)
            {
                if (gameMode.currentMode != GameModes.Continuous)
                    gameMode.currentMode = GameModes.Continuous;
            }

            int level = GetComponentInParent<WorldPortalProperties>().SceneIndex;
            GameManager gameManager = GameManager.instance;
            gameManager.lastPortalBuildIndex = level;
            gameManager.lastMode = gameManager.gameMode.currentMode;

            EventManager.OnTriggerTransition(level);
            pmc.ToggleMenuMovement(true);
        }
    }
}
