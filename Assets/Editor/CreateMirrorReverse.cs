namespace MirrorReverseHelperClasses
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;
    using UnityEngine.Assertions;
    public class CreateMirrorReverse
    {
        private string sceneName = "",
            originalScenePath = "", reverseScenePath = "", mirrorScenePath = "", reverseMirrorScenePath = "",
            originalSpawnPath = "", reverseSpawnPath = "", mirrorSpawnPath = "", reverseMirrorSpawnPath = "";
        public CreateMirrorReverse(string scene) { sceneName = scene; }
        public void CreateScenes()
        {
            GetAllPaths();
            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            CreateReverseScene();
            CreateMirrorScene();
            CreateReverseMirrorScene();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
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
            Directory.CreateDirectory(reverseScenePath.GetFullPath().GetDirectory());
            File.Copy(originalScenePath.GetFullPath(), reverseScenePath.GetFullPath(), true);
            Directory.CreateDirectory(reverseSpawnPath.GetFullPath().GetDirectory());
            File.Copy(originalSpawnPath.GetFullPath(), reverseSpawnPath.GetFullPath(), true);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            ReverseHelper.NewReverseScene(reverseScenePath, reverseSpawnPath);
        }
        private void CreateMirrorScene()
        {
            Directory.CreateDirectory(mirrorScenePath.GetFullPath().GetDirectory());
            File.Copy(originalScenePath.GetFullPath(), mirrorScenePath.GetFullPath(), true);
            Directory.CreateDirectory(mirrorSpawnPath.GetFullPath().GetDirectory());
            File.Copy(originalSpawnPath.GetFullPath(), mirrorSpawnPath.GetFullPath(), true);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            MirrorHelper.NewMirrorScene(mirrorScenePath, mirrorSpawnPath);
        }
        private void CreateReverseMirrorScene()
        {
            Directory.CreateDirectory(reverseMirrorScenePath.GetFullPath().GetDirectory());
            File.Copy(reverseScenePath.GetFullPath(), reverseMirrorScenePath.GetFullPath(), true);
            Directory.CreateDirectory(reverseMirrorSpawnPath.GetFullPath().GetDirectory());
            File.Copy(reverseSpawnPath.GetFullPath(), reverseMirrorSpawnPath.GetFullPath(), true);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            MirrorHelper.NewMirrorScene(reverseMirrorScenePath, reverseMirrorSpawnPath);
        }
    }
    public static class FilePathHelperExtensions
    {
        public static string GetFullPath(this string path)
        { return Application.dataPath + path.Substring(6); }
        public static string GetDirectory(this string path)
        { return path.Substring(0, path.LastIndexOfAny(@"/\".ToCharArray())); }
        public static string GetSceneName(this string path)
        { return path.GetFileName().RemoveFileExtension(); }
        public static string GetFileName(this string path)
        { return path.Substring(path.LastIndexOfAny(@"/\".ToCharArray()) + 1); }
        public static string RemoveFileExtension(this string path)
        { return path.Substring(0, path.LastIndexOf('.')); }
    }
    public static class AssertionExtensions
    {
        public static void AssertTrue(this bool value)
        { Assert.IsTrue(value); }
        public static void AssertFalse(this bool value)
        { Assert.IsFalse(value); }
        public static void AssertEqual<T>(this T value, T other)
        { Assert.AreEqual(other, value); }
        public static void AssertNotEqual<T>(this T value, T other)
        { Assert.AreNotEqual(other, value); }
    }
    public class ReverseHelper
    {
        private Scene scene;
        private GameObject spawn = null;
        private List<RingProperties> theRings = null;
        private RingProperties[] sortedRings = null;
        private Vector3 startRingPosition = Vector3.zero;
        private SerializedObject so = null;
        private ReverseHelper(Scene inScene, GameObject inSpawn)
        {
            scene = inScene;
            spawn = inSpawn;
        }
        public static void NewReverseScene(string scenePath, string spawnPath)
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            Scene reverseScene = SceneManager.GetActiveScene();
            GameObject reverseSpawn = AssetDatabase.LoadAssetAtPath<GameObject>(spawnPath);
            new ReverseHelper(reverseScene, reverseSpawn).ReverseScene();
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
            AssetDatabase.Refresh();
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
                nextRing.lastRingInScene.AssertTrue();
                exitRing.lastRingInScene.AssertTrue();
                startRing.lastRingInScene.AssertFalse();
                exitRing.nextScene.AssertEqual(1);
                nextRing.nextScene.AssertNotEqual(1);
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
            startRingSO = new SerializedObject(startRing);
            exitRingSO.FindProperty("positionInOrder").intValue = startRingSO.FindProperty("positionInOrder").intValue + 1;
            exitRingSO.ApplyModifiedProperties();
            nextRingSO = new SerializedObject(nextRing);
            nextRingSO.FindProperty("positionInOrder").intValue = exitRingSO.FindProperty("positionInOrder").intValue - 1;
            nextRingSO.ApplyModifiedProperties();
            startRingSO.FindProperty("positionInOrder").intValue = 2;
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
                nextRing.parent.AssertEqual(exitRing.parent);
                startRing.parent.AssertEqual(nextRing.parent);
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
        private void SetSpawnPoint()
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
            SetSpawnPoint();
        }
        private void ReverseRingPaths()
        {
            RingSetupScript ringParent = FindRingSetupScript();
            if (null != ringParent)
                for (GameDifficulties gameDifficulty = 0; GameDifficulties.GameDifficultiesSize != gameDifficulty; ++gameDifficulty)
                    ReverseRingPath(ringParent.GetRingDifficultyParent(gameDifficulty));
        }
    }
    public class MirrorHelper
    {
        private Scene scene;
        private GameObject spawn = null;
        private MirrorHelper(Scene inScene, GameObject inSpawn)
        {
            scene = inScene;
            spawn = inSpawn;
        }
        public static void NewMirrorScene(string scenePath, string spawnPath)
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            Scene mirrorScene = SceneManager.GetActiveScene();
            GameObject mirrorSpawn = AssetDatabase.LoadAssetAtPath<GameObject>(spawnPath);
            new MirrorHelper(mirrorScene, mirrorSpawn).MirrorScene();
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
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
        private void MirrorRootObjects()
        {
            List<Transform> rootTransforms = new List<Transform>();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
                rootTransforms.Add(rootObject.transform);
            Transform go = new GameObject("TEMPMIRRORROOT").transform;
            go.parent = null;
            go.localPosition = spawn.transform.localPosition;
            go.localRotation = spawn.transform.localRotation;
            go.localScale = spawn.transform.localScale;
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
            Vector3 tmpV3;
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
            Transform tmpT;
            BoxCollider tmpBc;
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
    public static class TransformCleaner
    {
        public static void CleanTransforms()
        {
            EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
            Scene scene;
            Queue<Transform> transforms = new Queue<Transform>();
            GameObject[] rootObjects;
            Transform transform;
            string originalScenePath = SceneManager.GetActiveScene().path;
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
            int i, j;
            for (i = LevelSelectOptions.MirroredBuildOffset; i < buildScenes.Length; ++i)
            {
                EditorSceneManager.OpenScene(buildScenes[i].path, OpenSceneMode.Single);
                scene = SceneManager.GetActiveScene();
                rootObjects = scene.GetRootGameObjects();
                foreach (GameObject rootObject in rootObjects)
                    transforms.Enqueue(rootObject.transform);
                while (transforms.Count > 0)
                {
                    transform = transforms.Dequeue();
                    for (j = 0; j < transform.childCount; ++j)
                        transforms.Enqueue(transform.GetChild(j));
                    CleanTransform(transform);
                }
                EditorSceneManager.MarkAllScenesDirty();
                EditorSceneManager.SaveOpenScenes();
            }
            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
        }
        private static Vector3 CleanVector(Vector3 dirty)
        {
            Vector3 clean = dirty;
            clean.x = CleanFloat(clean.x);
            clean.y = CleanFloat(clean.y);
            clean.z = CleanFloat(clean.z);
            return clean;
        }
        private const float epsilon = 0.0000001f;
        private static float CleanFloat(float dirty)
        {
            float clean = dirty;
            float round = Mathf.Round(clean);
            if (Mathf.Abs(clean - round) < epsilon)
                clean = round;
            return clean;
        }
        private static void CleanTransform(Transform transform)
        {
            Quaternion rotation = transform.localRotation;
            Vector3 position = transform.localPosition;
            Vector3 scale = transform.localScale;
            Vector3 euler = rotation.eulerAngles;
            euler = CleanVector(euler);
            rotation = Quaternion.identity;
            rotation = Quaternion.Euler(euler);
            transform.localRotation = rotation;
            position = CleanVector(position);
            scale = CleanVector(scale);
            transform.localPosition = position;
            transform.localScale = scale;
        }
    }
}