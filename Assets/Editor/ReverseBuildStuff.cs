using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
public class ReverseBuildStuff : ScriptableWizard
{
    public Transform spawnPrefab = null;
    public string source = "", destination = "";
    [MenuItem("Cybersurf Tools/Create Reversed Scene...")]
    private static void ProcessReverse()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string currentScenePath = activeScene.path;
        string reverseScenePath = activeScene.path;
        reverseScenePath = reverseScenePath.Substring(0, reverseScenePath.LastIndexOf('.'));
        int slash = reverseScenePath.LastIndexOfAny("/\\".ToCharArray());
        reverseScenePath = reverseScenePath.Substring(0, slash) + "/ReverseLevels" + reverseScenePath.Substring(slash) + "Reverse.unity";
        ReverseBuildStuff wizard = DisplayWizard<ReverseBuildStuff>("Create Reversed Scene", "Create Scene");
        wizard.source = activeScene.path;
        wizard.destination = reverseScenePath;
        string[] prefabSearch = AssetDatabase.FindAssets(activeScene.name + "Spawn");
        if (null != prefabSearch && prefabSearch.Length > 0)
            wizard.spawnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(prefabSearch[0])).transform;
        wizard.OnWizardUpdate();
    }
    public void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        if (source.StartsWith("Assets/") && destination.StartsWith("Assets/"))
            if (source.EndsWith(".unity") && destination.EndsWith(".unity"))
                if (System.IO.File.Exists(Application.dataPath + source.Substring(source.IndexOfAny("/\\".ToCharArray()))))
                    isValid = null != spawnPrefab;
                else
                    errorString = "source scene does not exist";
            else
                errorString = "filename must end with .unity";
        else
            errorString = "path must start at Assets/";
    }
    private void OnWizardCreate()
    {
        if (null != spawnPrefab)
        {
            Scene reverseScene = InitReverseScene();
            ReverseRingPaths(reverseScene);
            SaveReverseScene(reverseScene);
        }
    }
    private T FindComponent<T>(Scene scene) where T : Component
    {
        Queue<GameObject> searchQueue = new Queue<GameObject>();
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
            searchQueue.Enqueue(rootObject);
        T tmp = null;
        while (searchQueue.Count > 0)
            if (null != (tmp = searchQueue.Dequeue().GetComponent<T>()))
                return tmp;
        return null;
    }
    private List<RingProperties> GetRings(GameObject ringParent)
    {
        RingProperties[] ringProperties = ringParent.GetComponentsInChildren<RingProperties>(true);
        List<RingProperties> rings = new List<RingProperties>();
        foreach (RingProperties ringProperty in ringProperties)
            if (1 == ringProperty.GetComponentsInParent<RingProperties>(true).Length)
                rings.Add(ringProperty);
        return rings;
    }
    private RingProperties[] ListToSortedArray(List<RingProperties> rings)
    {
        int minPosition, maxPosition;
        GetPositionRange(rings, out minPosition, out maxPosition);
        // TODO : Sort rings from Min to Max and fix duplicates and gaps
        return rings.ToArray();
    }
    private RingProperties[] ReversePositionOrder(List<RingProperties> rings)
    {
        int minPosition, maxPosition;
        GetPositionRange(rings, out minPosition, out maxPosition);
        foreach (RingProperties ring in rings)
            ring.positionInOrder = minPosition + maxPosition - ring.positionInOrder;
        return ListToSortedArray(rings);
    }
    private void GetPositionRange(List<RingProperties> rings, out int minPosition, out int maxPosition)
    {
        maxPosition = int.MinValue;
        minPosition = int.MaxValue;
        foreach (RingProperties ring in rings)
        {
            if (ring.positionInOrder > maxPosition)
                maxPosition = ring.positionInOrder;
            if (ring.positionInOrder < minPosition)
                minPosition = ring.positionInOrder;
        }
    }
    private void ProcessLastRings(RingProperties[] sortedRings)
    {
        // TODO
    }
    private void ReverseRingPath(GameObject ringParent)
    {
        ProcessLastRings(ReversePositionOrder(GetRings(ringParent)));
    }
    private void ReverseRingPaths(Scene scene)
    {
        RingSetupScript ringParent = FindComponent<RingSetupScript>(scene);
        for (GameDifficulties gameDifficulty = 0; GameDifficulties.GameDifficultiesSize != gameDifficulty; ++gameDifficulty)
            ReverseRingPath(ringParent.GetRingDifficultyParent(gameDifficulty));
    }
    private Scene InitReverseScene()
    {
        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
        string currentSceneFullPath = Application.dataPath + source.Substring(source.IndexOfAny("/\\".ToCharArray()));
        string reverseSceneFullPath = Application.dataPath + destination.Substring(destination.IndexOfAny("/\\".ToCharArray()));
        System.IO.File.Copy(currentSceneFullPath, reverseSceneFullPath, true);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.OpenScene(destination, OpenSceneMode.Single));
        return SceneManager.GetActiveScene();
    }
    private void SaveReverseScene(Scene scene)
    {
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.OpenScene(source, OpenSceneMode.Single));
        EditorSceneManager.SaveOpenScenes();
    }
}