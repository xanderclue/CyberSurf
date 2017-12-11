using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
public class RingProcessorWizard : ScriptableWizard
{
    [Header("Bonus Time and Queue Order Settings")]
    [Range(10.0f, 50.0f)]
    public float targetVelocity = 30.0f;
    [Range(-1.0f, 1.0f), Tooltip("Increase or decrease the target bonus time based off of this percentage of the calculated bonus time.")]
    public float timePercentModifier = 0.0f;
    public int startPositionInOrder = 1;

    [Header("Last Ring Settings")]
    public int nextSceneIndex = 1;

    [Header("Drag Rings and Mutators In Desired Order Here")]
    public RingSetupScript ringsParent = null;
    private RingProperties[] ringsToProcess = null;
    private Vector3 prevPosition, currPosition;
    private int currQueuePosition = 0;
    private RingProperties previousRing = null, currentRing = null;

    [MenuItem("Cybersurf Tools/Ring Processor Wizard")]
    private static void ProcessRings() => DisplayWizard<RingProcessorWizard>("Ring Processor Wizard", "Update");
    private bool Init()
    {
        if (null == ringsToProcess || ringsToProcess.Length < 4)
            return false;
        currPosition = prevPosition = Vector3.zero;
        currQueuePosition = startPositionInOrder;
        for (int i = 0; i < ringsToProcess.Length; ++i)
        {
            previousRing = ringsToProcess[i];
            if (null != previousRing)
            {
                prevPosition = previousRing.transform.position;
                break;
            }
        }
        return null != previousRing;
    }
    private void SetProperties()
    {
        for (int i = 1; i < ringsToProcess.Length; ++i)
        {
            currentRing = ringsToProcess[i];
            currPosition = currentRing.transform.position;
            previousRing.bonusTime = Vector3.Distance(prevPosition, currPosition) * (timePercentModifier + 1.0f) / targetVelocity;
            previousRing.positionInOrder = currQueuePosition;
            previousRing.nextScene = -1;
            previousRing.lastRingInScene = false;
            UnityEditorInternal.ComponentUtility.CopyComponent(previousRing);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(previousRing);
            ++currQueuePosition;
            previousRing = currentRing;
            prevPosition = currPosition;
        }
        previousRing = ringsToProcess[ringsToProcess.Length - 2];
        previousRing.lastRingInScene = true;
        previousRing.nextScene = nextSceneIndex;
        UnityEditorInternal.ComponentUtility.CopyComponent(previousRing);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(previousRing);
        currentRing.lastRingInScene = true;
        currentRing.nextScene = LevelSelectOptions.HubWorldBuildIndex;
        currentRing.bonusTime = Vector3.Distance(ringsToProcess[0].transform.position, currentRing.transform.position) * (timePercentModifier + 1.0f) / targetVelocity;
        currentRing.positionInOrder = currQueuePosition;
        UnityEditorInternal.ComponentUtility.CopyComponent(currentRing);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(currentRing);
    }
    private void OnWizardCreate()
    {
        if (null == ringsParent) return;
        for (int i = 0; i < (int)GameDifficulty.GameDifficultiesSize; ++i)
        {
            ringsToProcess = GetRings(ringsParent.GetRingDifficultyParent((GameDifficulty)i).transform);
            if (Init())
                SetProperties();
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
    private class RingPropertiesSiblingIndexComparer : IComparer<RingProperties>
    { public int Compare(RingProperties x, RingProperties y) => x.transform.GetSiblingIndex() - y.transform.GetSiblingIndex(); }
    private static RingProperties[] GetRings(Transform parent)
    {
        List<RingProperties> rings = new List<RingProperties>();
        int childCount = parent.childCount;
        for (int i = 0; i < parent.childCount; ++i)
            rings.Add(parent.GetChild(i).GetComponent<RingProperties>());
        rings.Sort(new RingPropertiesSiblingIndexComparer());
        return rings.ToArray();
    }
}