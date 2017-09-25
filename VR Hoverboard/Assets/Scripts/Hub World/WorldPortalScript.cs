using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPortalScript : MonoBehaviour
{
    [SerializeField] bool isDemoMode = false;

    System.Type boxCollider;
    PlayerMenuController pmc;

    ManagerClasses.GameMode gameMode;
    LevelMenu levelMenuScript;

    private void Start()
    {
        boxCollider = typeof(UnityEngine.BoxCollider);
        pmc = GameManager.player.GetComponent<PlayerMenuController>();
        gameMode = GameManager.instance.gameMode;

        levelMenuScript = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == boxCollider && other.gameObject.tag == "Board")
        {
            if (isDemoMode && levelMenuScript != null)
            {
                while (gameMode.currentMode != GameModes.Continuous)
                    levelMenuScript.NextGameMode();
            }

            int level = GetComponentInParent<WorldPortalProperties>().sceneIndex;
            GameManager gameManager = GameManager.instance;
            gameManager.lastLevel = level;
            gameManager.lastMode = gameManager.gameMode.currentMode;


            EventManager.OnTriggerTransition(level);
            pmc.ToggleMenuMovement(true);
        }
    }
}
