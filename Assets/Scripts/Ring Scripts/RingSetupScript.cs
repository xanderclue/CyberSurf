using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSetupScript : MonoBehaviour
{
    [SerializeField] GameObject[] ringDifficultyParents;
    Transform[] ringTransforms;
    arrowPointAtUpdater arrowScript;
    RingProperties[] sortedRings;

    //state of what the rings should be setup as determined by gamemode
    GameModes mode;
    GameDifficulties difficulty;
    ManagerClasses.RoundTimer roundTimer;

    void Start()
    {
        arrowScript = GameManager.player.GetComponentInChildren<arrowPointAtUpdater>();
        mode = GameManager.instance.gameMode.currentMode;
        difficulty = GameManager.instance.gameDifficulty.currentDifficulty;
        roundTimer = GameManager.instance.roundTimer;

        //turn off the parents, just in case they got left on
        foreach (GameObject item in ringDifficultyParents)
        {
            if (item != null && item.activeSelf)
                item.SetActive(false);
        }

        //turn on the parent depending on difficulty setting
        switch(difficulty)
        {
            case GameDifficulties.Easy:
                if (ringDifficultyParents[0] != null)
                    ringDifficultyParents[0].SetActive(true);
                else
                    print("EASY RINGS PARENT NOT SET IN THE RINGS PARENT OBJECT!");
                break;
            case GameDifficulties.Normal:
                if (ringDifficultyParents[1] != null)
                    ringDifficultyParents[1].SetActive(true);
                else
                    print("NORMAL RINGS PARENT NOT SET IN THE RINGS PARENT OBJECT!");
                break;
            case GameDifficulties.Hard:
                if (ringDifficultyParents[2] != null)
                    ringDifficultyParents[2].SetActive(true);
                else
                    print("HARD RINGS PARENT NOT SET IN THE RINGS PARENT OBJECT!");
                break;
            default:
                Debug.LogWarning("Missing case: \"" + difficulty.ToString("F") + "\"");
                break;
        }

        RingProperties[] rings;

        //use a list to remove rings we won't need depending on game mode
        List<RingProperties> ringList = new List<RingProperties>();

        rings = GetComponentsInChildren<RingProperties>();

        //insertion sort the rings according to their position in order
        InsertionSort(rings, rings.GetLength(0));

        //set our sorted rings to our ring list
        foreach (RingProperties rp in rings)
        {
            ringList.Add(rp);
        }

        //update our ring list depending on game mode
        setRingsMode(ringList);

        //assign the transforms from the sorted list
        ringTransforms = new Transform[ringList.Count];

        for (int i = 0; i < ringList.Count; i++)
            ringTransforms[i] = ringList[i].transform;

        sortedRings = rings;

        //set the startup bonus time for the player if in curse mode
        if (mode == GameModes.Cursed)
            SetupStartBonusTime();

        //setup our arrowScript if we're using it
        if (arrowScript != null)
        {
            arrowScript.thingsToLookAt = ringTransforms;
            arrowScript.sortedRings = rings;           
        }
        if (ringTransforms != null)
        {
            gameObject.GetComponent<ringPathMaker>().init(ringTransforms);
        }
    }

    void setRingsMode(List<RingProperties> rings)
    {
        switch (mode)
        {
            case GameModes.Continuous:
                arrowScript.currentlyLookingAt = 1;
                break;
            case GameModes.Cursed:
                RingProperties lastRing = rings[rings.Count - 1];
                RingProperties nextToLastRing = rings[rings.Count - 2];

                if (lastRing.nextScene != 1)
                {
                    lastRing.gameObject.SetActive(false);
                    rings.Remove(lastRing);
                }
                else
                {
                    nextToLastRing.gameObject.SetActive(false);
                    rings.Remove(nextToLastRing);
                }
                arrowScript.currentlyLookingAt = 1;
                break;
            case GameModes.Free:
                for (int i = 0; i < rings.Count - 2; i++)
                {
                    rings[i].gameObject.SetActive(false);
                }

                arrowScript.currentlyLookingAt = rings.Count - 1;
                break;
            default:
                Debug.LogWarning("Missing case: \"" + mode.ToString("F") + "\"");
                break;
        }
    }

    void InsertionSort(RingProperties[] rings, int arrayLength)
    {
        int currRing = 1;
        while (currRing < arrayLength)
        {
            RingProperties storedRing = rings[currRing];

            int compareRing = currRing - 1;
            while (compareRing >= 0 && rings[compareRing].positionInOrder > storedRing.positionInOrder)
            {
                rings[compareRing + 1] = rings[compareRing];
                --compareRing;
            }

            rings[compareRing + 1] = storedRing;
            ++currRing;
        }
    }

    void SetupStartBonusTime()
    {
        ManagerClasses.PlayerMovementVariables currPMV = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables;

        float currentAverage = (currPMV.minSpeed + currPMV.restingSpeed + currPMV.maxSpeed) / 3f;
        float startingBonusTime = 5f;

        float distanceToRing = Vector3.Distance(GameManager.player.GetComponent<Transform>().position, sortedRings[0].GetComponent<Transform>().position);

        roundTimer.TimeLeft = (distanceToRing / currentAverage) + startingBonusTime;
    }

    private void OnDisable()
    {
        if (arrowScript != null)
        {
            arrowScript.thingsToLookAt = null;
            arrowScript.currentlyLookingAt = -1;
        }
    }

}
