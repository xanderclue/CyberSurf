using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class ReverseBuildStuff : ScriptableWizard
{
    public Transform spawnPrefab = null, reverseSpawnPrefab = null;
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
        GameObject prefabSearch = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SpawnPoints/" + activeScene.name + "Spawn.prefab");
        if (null != prefabSearch)
            wizard.spawnPrefab = prefabSearch.transform;
        prefabSearch = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SpawnPoints/Reverse/" + activeScene.name + "ReverseSpawn.prefab");
        if (null != prefabSearch)
            wizard.reverseSpawnPrefab = prefabSearch.transform;
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
    private class RingPropertiesPositionInOrderComparer : IComparer<RingProperties> { public int Compare(RingProperties x, RingProperties y) { return x.positionInOrder - y.positionInOrder; } }
    private RingProperties[] ListToSortedArray(List<RingProperties> rings)
    {
        RingProperties[] ringArr = rings.ToArray();
        System.Array.Sort(ringArr, new RingPropertiesPositionInOrderComparer());
        SerializedObject so = null;
        for (int i = 0; i < ringArr.Length; ++i)
        {
            so = new SerializedObject(ringArr[i]);
            so.FindProperty("positionInOrder").intValue = i + 1;
            so.ApplyModifiedProperties();
        }
        return ringArr;
    }
    private RingProperties[] ReversePositionOrder(List<RingProperties> rings)
    {
        int minPosition, maxPosition;
        GetPositionRange(rings, out minPosition, out maxPosition);
        SerializedObject so = null;
        SerializedProperty sp = null;
        foreach (RingProperties ring in rings)
        {
            so = new SerializedObject(ring);
            sp = so.FindProperty("positionInOrder");
            sp.intValue = minPosition + maxPosition - sp.intValue;
            so.ApplyModifiedProperties();
        }
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
    private void DecrementAllPositions(RingProperties[] rings)
    {
        SerializedObject so = null;
        foreach (RingProperties ring in rings)
        {
            so = new SerializedObject(ring);
            --so.FindProperty("positionInOrder").intValue;
            so.ApplyModifiedProperties();
        }
    }
    private Vector3 ProcessRingEnds(RingProperties[] sortedRings)
    {
        RingProperties exitRing = null, nextRing = null, startRing = null;
        startRing = sortedRings[sortedRings.Length - 1];
        exitRing = sortedRings[0];
        nextRing = sortedRings[1];
        if (1 == nextRing.nextScene)
        {
            exitRing = nextRing;
            nextRing = sortedRings[0];
        }
        bool ogAssertRaise = Assert.raiseExceptions;
        Assert.raiseExceptions = true;
        try
        {
            Assert.IsTrue(nextRing.lastRingInScene);
            Assert.IsTrue(exitRing.lastRingInScene);
            Assert.IsFalse(startRing.lastRingInScene);
            Assert.AreEqual(1, exitRing.nextScene);
            Assert.AreNotEqual(1, nextRing.nextScene);
        }
        catch
        {
            Debug.LogError("Reverse scene setup has failed to finish processing rings. Rings may " +
                "not have been setup correctly. Make sure rings have correct position numbers " +
                "and that the last two rings (Next and Exit rings) are properly setup for scene " +
                "transitions. Both rings should have lastRingInScene set to true, and all other " +
                "rings should have it set to false. The nextScene field for the Next ring should " +
                "be set to a scene number that isn't the hub world, and the nextScene field for " +
                "the Exit ring should only be set for the hub world. Be sure to check the ring " +
                "setups for all difficulty levels.");
            return spawnPrefab.position + spawnPrefab.forward;
        }
        finally
        {
            Assert.raiseExceptions = ogAssertRaise;
        }
        SerializedObject exitRingSO = null, nextRingSO = null, startRingSO = null;
        exitRingSO = new SerializedObject(exitRing);
        nextRingSO = new SerializedObject(nextRing);
        startRingSO = new SerializedObject(startRing);
        exitRingSO.FindProperty("positionInOrder").intValue = startRingSO.FindProperty("positionInOrder").intValue + 1;
        nextRingSO.FindProperty("positionInOrder").intValue = exitRingSO.FindProperty("positionInOrder").intValue - 1;
        startRingSO.FindProperty("positionInOrder").intValue = 2;
        exitRingSO.ApplyModifiedProperties();
        nextRingSO.ApplyModifiedProperties();
        startRingSO.ApplyModifiedProperties();
        DecrementAllPositions(sortedRings);
        MoveExitRings(exitRing.transform, nextRing.transform, startRing.transform);
        return startRing.transform.position;
    }
    private void MoveExitRings(Transform exitRing, Transform nextRing, Transform startRing)
    {
        bool ogAssertRaise = Assert.raiseExceptions;
        Assert.raiseExceptions = true;
        try
        {
            Assert.AreEqual(exitRing.parent, nextRing.parent);
            Assert.AreEqual(nextRing.parent, startRing.parent);
        }
        catch { Debug.LogError("start/next/exit rings of the same difficulty must be have the same immediate parent"); return; }
        finally { Assert.raiseExceptions = ogAssertRaise; }
        exitRing.parent = nextRing;
        Vector3 position = startRing.position, localScale = startRing.localScale;
        Quaternion rotation = startRing.rotation;
        startRing.position = nextRing.position;
        startRing.rotation = nextRing.rotation;
        startRing.localScale = nextRing.localScale;
        nextRing.position = position;
        nextRing.rotation = rotation;
        exitRing.parent = nextRing.parent;
        nextRing.localScale = localScale;
    }
    private void PointSpawnAtStart(Vector3 ringPosition)
    {
        if (null != reverseSpawnPrefab)
        {
            reverseSpawnPrefab.LookAt(ringPosition, Vector3.up);
            Vector3 euler = reverseSpawnPrefab.eulerAngles;
            euler.z = euler.x = 0.0f;
            reverseSpawnPrefab.eulerAngles = euler;
        }
    }
    private void ReverseRingPath(GameObject ringParent)
    {
        PointSpawnAtStart(ProcessRingEnds(ReversePositionOrder(GetRings(ringParent))));
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