using System.Collections.Generic;
using UnityEngine;
using Xander.Debugging;
using Xander.NullConversion;
public class RingSetupScript : MonoBehaviour
{
    [SerializeField] private GameObject[] ringDifficultyParents = null;
    private Transform[] ringTransforms = null;
    private arrowPointAtUpdater arrowScript = null;
    private RingProperties[] sortedRings = null;
    private GameModes mode;
    private GameDifficulties difficulty;
    private ManagerClasses.RoundTimer roundTimer = null;
    private void Start()
    {
        arrowScript = GameManager.player.GetComponentInChildren<arrowPointAtUpdater>();
        mode = GameManager.instance.gameMode.currentMode;
        difficulty = GameManager.instance.gameDifficulty.currentDifficulty;
        roundTimer = GameManager.instance.roundTimer;
        foreach (GameObject item in ringDifficultyParents)
            item.ConvertNull()?.SetActive(false);
        ringDifficultyParents[(int)difficulty].SetActive(true);
        List<RingProperties> ringList = new List<RingProperties>();
        RingProperties[] rings = GetComponentsInChildren<RingProperties>();
        InsertionSort(rings);
        foreach (RingProperties rp in rings)
            ringList.Add(rp);
        setRingsMode(ringList);
        ringTransforms = new Transform[ringList.Count];
        for (int i = 0; i < ringList.Count; ++i)
            ringTransforms[i] = ringList[i].transform;
        sortedRings = rings;
        if (GameModes.Cursed == mode)
            SetupStartBonusTime();
        if (null != arrowScript)
        {
            arrowScript.thingsToLookAt = ringTransforms;
            arrowScript.sortedRings = rings;
        }
        if (null != ringTransforms)
            GetComponent<ringPathMaker>().Init(ringTransforms);
    }
    private void setRingsMode(List<RingProperties> rings)
    {
        switch (mode)
        {
            case GameModes.Continuous:
                arrowScript.currentlyLookingAt = 1;
                break;
            case GameModes.Cursed:
                RingProperties lastRing = rings[rings.Count - 1];
                RingProperties nextToLastRing = rings[rings.Count - 2];
                if (1 != lastRing.nextScene)
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
                for (int i = 0; i < rings.Count - 2; ++i)
                    rings[i].gameObject.SetActive(false);
                arrowScript.currentlyLookingAt = rings.Count - 1;
                break;
            case GameModes.Race:
                arrowScript.currentlyLookingAt = 1;
                break;
            default:
                Debug.LogWarning("Missing case: \"" + mode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
    }
    private void InsertionSort(RingProperties[] rings)
    {
        int currRing = 1;
        while (currRing < rings.Length)
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
    private void SetupStartBonusTime()
    {
        ManagerClasses.PlayerMovementVariables currPMV = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables;
        roundTimer.TimeLeft = (3.0f * Vector3.Distance(GameManager.player.GetComponent<Transform>().position, sortedRings[0].GetComponent<Transform>().position) / (currPMV.minSpeed + currPMV.restingSpeed + currPMV.maxSpeed)) + 5.0f;
    }
    private void OnDisable()
    {
        if (null != arrowScript)
        {
            arrowScript.thingsToLookAt = null;
            arrowScript.currentlyLookingAt = -1;
        }
    }
}