using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using MirrorReverseHelperClasses;
public class CreateMirrorReverse : ScriptableWizard
{
    public string sceneName = "";
    private string
        originalScenePath = "", reverseScenePath = "", mirrorScenePath = "", reverseMirrorScenePath = "",
        originalSpawnPath = "", reverseSpawnPath = "", mirrorSpawnPath = "", reverseMirrorSpawnPath = "";
    [MenuItem("Cybersurf Tools/Create Mirror and Reverse...")]
    private static void ProcessMirrorReverse()
    {
        CreateMirrorReverse wizard = DisplayWizard<CreateMirrorReverse>("Create Mirror and Reverse Scenes", "Create Scenes");
        wizard.sceneName = SceneManager.GetActiveScene().path.GetFileName().RemoveFileExtension();
        wizard.OnWizardUpdate();
    }
    private void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        originalScenePath = string.Format("Assets/Scenes/{0}.unity", sceneName);
        originalSpawnPath = string.Format("Assets/Prefabs/SpawnPoints/{0}Spawn.prefab", sceneName);
        if (!File.Exists(originalScenePath.GetFullPath()))
            errorString = string.Format("Cannot find {0} scene @ {1}", sceneName, originalScenePath);
        else if (!File.Exists(originalSpawnPath.GetFullPath()))
            errorString = string.Format("Cannot find {0} spawn point @ {1}", sceneName, originalSpawnPath);
        else
            isValid = true;
    }
    private void OnWizardCreate()
    {
        EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        GetAllPaths();
        CreateReverseScene();
        CreateMirrorScene();
        CreateReverseMirrorScene();
        EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
    }
    private void GetAllPaths()
    {
        originalScenePath = string.Format("Assets/Scenes/{0}.unity", sceneName);
        reverseScenePath = string.Format("Assets/Scenes/ReverseLevels/{0}Reverse.unity", sceneName);
        mirrorScenePath = string.Format("Assets/Scenes/MirrorLevels/{0}Mirror.unity", sceneName);
        reverseMirrorScenePath = string.Format("Assets/Scenes/ReverseLevels/MirrorLevels/{0}ReverseMirror.unity", sceneName);
        originalSpawnPath = string.Format("Assets/Prefabs/SpawnPoints/{0}Spawn.prefab", sceneName);
        reverseSpawnPath = string.Format("Assets/Prefabs/SpawnPoints/ReverseLevels/{0}ReverseSpawn.prefab", sceneName);
        mirrorSpawnPath = string.Format("Assets/Prefabs/SpawnPoints/MirrorLevels/{0}MirrorSpawn.prefab", sceneName);
        reverseMirrorSpawnPath = string.Format("Assets/Prefabs/SpawnPoints/ReverseLevels/MirrorLevels/{0}ReverseMirrorSpawn.prefab", sceneName);
    }
    private void CreateReverseScene()
    {
        File.Copy(originalScenePath.GetFullPath(), reverseScenePath.GetFullPath(), true);
        File.Copy(originalSpawnPath.GetFullPath(), reverseSpawnPath.GetFullPath(), true);
        ReverseHelper.ReverseScene(reverseScenePath, reverseSpawnPath);
    }
    private void CreateMirrorScene()
    {
        File.Copy(originalScenePath.GetFullPath(), mirrorScenePath.GetFullPath(), true);
        File.Copy(originalSpawnPath.GetFullPath(), mirrorSpawnPath.GetFullPath(), true);
        MirrorHelper.MirrorScene(mirrorScenePath, mirrorSpawnPath);
    }
    private void CreateReverseMirrorScene()
    {
        File.Copy(reverseScenePath.GetFullPath(), reverseMirrorScenePath.GetFullPath(), true);
        File.Copy(reverseSpawnPath.GetFullPath(), reverseMirrorSpawnPath.GetFullPath(), true);
        MirrorHelper.MirrorScene(reverseMirrorScenePath, reverseMirrorSpawnPath);
    }
}
namespace MirrorReverseHelperClasses
{
    public static class FilePathHelper
    {
        public static string GetFullPath(this string path)
        {
            return Application.dataPath + path.Substring(6);
        }
        public static string GetFileName(this string path)
        {
            return path.Substring(path.LastIndexOfAny(@"/\".ToCharArray()) + 1);
        }
        public static string RemoveFileExtension(this string path)
        {
            return path.Substring(0, path.LastIndexOf('.'));
        }
    }
    public class ReverseHelper
    {
        private Scene scene;
        private GameObject spawn = null;
        private List<RingProperties> theRings = null;
        private RingProperties[] sortedRings = null;
        private Vector3 startRingPosition = Vector3.zero;
        public ReverseHelper(Scene inScene, GameObject inSpawn)
        {
            scene = inScene;
            spawn = inSpawn;
        }
        public static void ReverseScene(string scenePath, string spawnPath)
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            GameObject reverseSpawn = AssetDatabase.LoadAssetAtPath<GameObject>(spawnPath);
            new ReverseHelper(SceneManager.GetActiveScene(), reverseSpawn).ReverseScene();
        }
        private void ReverseScene()
        {
            SaveScene();
            SaveSpawn();
            ReverseRingPaths();
            SaveScene();
            SaveSpawn();
        }
        private void SaveScene()
        {
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveOpenScenes();
        }
        private void SaveSpawn()
        {
            EditorUtility.SetDirty(spawn);
            AssetDatabase.SaveAssets();
        }
        private RingSetupScript FindRingSetupScript()
        {
            Queue<GameObject> searchQueue = new Queue<GameObject>();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
                searchQueue.Enqueue(rootObject);
            RingSetupScript tmp = null;
            while (searchQueue.Count > 0)
                if (null != (tmp = searchQueue.Dequeue().GetComponent<RingSetupScript>()))
                    return tmp;
            return null;
        }
        private void GetRings(GameObject ringParent)
        {
            RingProperties[] ringProperties = ringParent.GetComponentsInChildren<RingProperties>(true);
            theRings = new List<RingProperties>();
            foreach (RingProperties ringProperty in ringProperties)
                if (1 == ringProperty.GetComponentsInParent<RingProperties>(true).Length)
                    theRings.Add(ringProperty);
        }
        private class RingPropertiesPositionInOrderComparer : IComparer<RingProperties>
        {
            public int Compare(RingProperties x, RingProperties y)
            {
                return x.positionInOrder - y.positionInOrder;
            }
        }
        private void GetSortedRings()
        {
            sortedRings = theRings.ToArray();
            System.Array.Sort(sortedRings, new RingPropertiesPositionInOrderComparer());
            SerializedObject so = null;
            for (int i = 0; i < sortedRings.Length; ++i)
            {
                so = new SerializedObject(sortedRings[i]);
                so.FindProperty("positionInOrder").intValue = i + 1;
                so.ApplyModifiedProperties();
            }
        }
        private void ReversePositionOrder()
        {
            int minPosition, maxPosition;
            GetPositionRange(out minPosition, out maxPosition);
            SerializedObject so = null;
            SerializedProperty sp = null;
            foreach (RingProperties ring in theRings)
            {
                so = new SerializedObject(ring);
                sp = so.FindProperty("positionInOrder");
                sp.intValue = minPosition + maxPosition - sp.intValue;
                so.ApplyModifiedProperties();
            }
        }
        private void GetPositionRange(out int minPosition, out int maxPosition)
        {
            maxPosition = int.MinValue;
            minPosition = int.MaxValue;
            foreach (RingProperties ring in theRings)
            {
                if (ring.positionInOrder > maxPosition)
                    maxPosition = ring.positionInOrder;
                if (ring.positionInOrder < minPosition)
                    minPosition = ring.positionInOrder;
            }
        }
        private void DecrementAllPositions()
        {
            SerializedObject so = null;
            foreach (RingProperties ring in sortedRings)
            {
                so = new SerializedObject(ring);
                --so.FindProperty("positionInOrder").intValue;
                so.ApplyModifiedProperties();
            }
        }
        private void ProcessRingEnds()
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
                startRingPosition = spawn.transform.position + spawn.transform.forward;
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
            DecrementAllPositions();
            MoveExitRings(exitRing.transform, nextRing.transform, startRing.transform);
            startRingPosition = startRing.transform.position;
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
        private void PointSpawnAtStart()
        {
            spawn.transform.LookAt(startRingPosition, Vector3.up);
            spawn.transform.eulerAngles = new Vector3(0.0f, spawn.transform.eulerAngles.y, 0.0f);
        }
        private void ReverseRingPath(GameObject ringParent)
        {
            GetRings(ringParent);
            ReversePositionOrder();
            GetSortedRings();
            ProcessRingEnds();
            PointSpawnAtStart();
        }
        private void ReverseRingPaths()
        {
            RingSetupScript ringParent = FindRingSetupScript();
            for (GameDifficulties gameDifficulty = 0; GameDifficulties.GameDifficultiesSize != gameDifficulty; ++gameDifficulty)
                ReverseRingPath(ringParent.GetRingDifficultyParent(gameDifficulty));
        }
    }
    public class MirrorHelper
    {
        private Scene scene;
        private GameObject spawn = null;
        public MirrorHelper(Scene inScene, GameObject inSpawn)
        {
            scene = inScene;
            spawn = inSpawn;
        }
        public static void MirrorScene(string scenePath, string spawnPath)
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            GameObject mirrorSpawn = AssetDatabase.LoadAssetAtPath<GameObject>(spawnPath);
            new MirrorHelper(SceneManager.GetActiveScene(), mirrorSpawn).MirrorScene();
        }
        private void MirrorScene()
        {
            SaveScene();
            SaveSpawn();
            MirrorRootObjects();
            InvertBoxColliders();
            SaveScene();
            SaveSpawn();
        }
        private void SaveScene()
        {
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveOpenScenes();
        }
        private void SaveSpawn()
        {
            EditorUtility.SetDirty(spawn);
            AssetDatabase.SaveAssets();
        }
        private void MirrorRootObjects()
        {
            Transform go = new GameObject("TEMPMIRRORROOT").transform;
            go.parent = null;
            go.localPosition = spawn.transform.localPosition;
            go.localRotation = spawn.transform.localRotation;
            go.localScale = spawn.transform.localScale;
            List<Transform> rootTransforms = new List<Transform>();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
                if (rootObject.transform != go)
                    rootTransforms.Add(rootObject.transform);
            foreach (Transform rootTransform in rootTransforms)
                rootTransform.parent = go;
            Vector3 theScale = go.localScale;
            theScale.x = -theScale.x;
            go.localScale = theScale;
            foreach (Transform rootTransform in rootTransforms)
                rootTransform.parent = null;
            Object.DestroyImmediate(go.gameObject);
        }
        private void InvertBoxColliders()
        {
            List<BoxCollider> boxColliders = GetBoxColliders();
            Vector3 tmpV3 = Vector3.zero;
            foreach (BoxCollider boxCollider in boxColliders)
            {
                tmpV3 = boxCollider.size;
                tmpV3.x = -tmpV3.x;
                boxCollider.size = tmpV3;
            }
        }
        private List<BoxCollider> GetBoxColliders()
        {
            Queue<Transform> transforms = GetRootTransforms();
            List<BoxCollider> boxColliders = new List<BoxCollider>();
            Transform tmpT = null;
            BoxCollider tmpBc = null;
            while (transforms.Count > 0)
            {
                tmpT = transforms.Dequeue();
                for (int i = 0; i < tmpT.childCount; ++i)
                    transforms.Enqueue(tmpT.GetChild(i));
                tmpBc = tmpT.GetComponent<BoxCollider>();
                if (null != tmpBc)
                    boxColliders.Add(tmpBc);
            }
            return boxColliders;
        }
        private Queue<Transform> GetRootTransforms()
        {
            Queue<Transform> transforms = new Queue<Transform>();
            GameObject[] gameObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in gameObjects)
                transforms.Enqueue(rootObject.transform);
            return transforms;
        }
    }
}