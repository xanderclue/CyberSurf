using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
public class RingProcessorWizard : ScriptableWizard
{
    [Header("Bonus Time and Queue Order Settings"), Range(10.0f, 50.0f)] public float targetVelocity = 30.0f;
    [Range(-1.0f, 1.0f), Tooltip("Increase or decrease the target bonus time based off of this percentage of the calculated bonus time.")] public float timePercentModifier = 0.0f;
    public int startPositionInOrder = 1;
    [Header("Last Ring Settings")] public bool setAsLastInScene = true;
    public int nextSceneIndex = 1;
    [Header("Drag Rings and Rotators In Desired Order Here")] public Object[] ringsToProcess = null;
    private Vector3 prevPosition, currPosition;
    private int currQueuePosition = 0;
    private GameObject previousGameObject = null, currentGameObject = null;
    [MenuItem("Cybersurf Tools/Ring Processor Wizard")]
    private static void ProcessRings()
    {
        DisplayWizard<RingProcessorWizard>("Ring Processor Wizard", "Update And Close", "Update");
    }
    private bool Init()
    {
        if (null == ringsToProcess || ringsToProcess.Length < 2)
            return false;
        currPosition = prevPosition = Vector3.zero;
        currQueuePosition = startPositionInOrder;
        for (int i = 0; i < ringsToProcess.Length; ++i)
        {
            previousGameObject = (GameObject)ringsToProcess[i];
            if (null != previousGameObject.GetComponent<RingProperties>())
            {
                prevPosition = previousGameObject.GetComponent<RingProperties>().transform.position;
                break;
            }
        }
        return null != previousGameObject;
    }
    private float CalculateBonusTime()
    {
        return Vector3.Distance(prevPosition, currPosition) * (timePercentModifier + 1.0f) / targetVelocity;
    }
    private void SetProperties()
    {
        RingProperties rp;
        for (int i = 1; i < ringsToProcess.Length; ++i)
        {
            currentGameObject = (GameObject)ringsToProcess[i];
            if (null != currentGameObject.GetComponent<RingProperties>())
            {
                currPosition = currentGameObject.transform.position;
                rp = previousGameObject.GetComponent<RingProperties>();
                rp.bonusTime = CalculateBonusTime();
                rp.positionInOrder = currQueuePosition;
                UnityEditorInternal.ComponentUtility.CopyComponent(rp);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(rp);
            }
            ++currQueuePosition;
            previousGameObject = currentGameObject;
            prevPosition = currPosition;
        }
        rp = currentGameObject.GetComponent<RingProperties>();
        if (null != rp)
        {
            if (setAsLastInScene)
            {
                rp.lastRingInScene = true;
                rp.nextScene = nextSceneIndex;
            }
            rp.bonusTime = 0.0f;
            rp.positionInOrder = currQueuePosition;
            UnityEditorInternal.ComponentUtility.CopyComponent(rp);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(rp);
        }
    }
    private void OnWizardCreate()
    {
        if (Init())
        {
            SetProperties();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
    private void OnWizardOtherButton()
    {
        if (Init())
        {
            helpString = "Rings Processed!";
            SetProperties();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        else
            helpString = "Not enough items in the Rings To Process array!";
    }
}