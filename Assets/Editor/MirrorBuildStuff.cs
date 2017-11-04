using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
public class MirrorBuildStuff : ScriptableWizard
{
    public Transform spawnPrefab = null;
    public string source = "", destination = "";
    [MenuItem("Cybersurf Tools/Create Mirrored Scene...")]
    private static void ProcessMirror()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string currentScenePath = activeScene.path;
        string mirrorScenePath = activeScene.path;
        mirrorScenePath = mirrorScenePath.Substring(0, mirrorScenePath.LastIndexOf('.'));
        int slash = mirrorScenePath.LastIndexOfAny("/\\".ToCharArray());
        mirrorScenePath = mirrorScenePath.Substring(0, slash) + "/MirrorLevels" + mirrorScenePath.Substring(slash) + "Mirror.unity";
        MirrorBuildStuff wizard = DisplayWizard<MirrorBuildStuff>("Create Mirrored Scene", "Create Scene");
        wizard.source = activeScene.path;
        wizard.destination = mirrorScenePath;
        GameObject prefabSearch = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SpawnPoints/" + activeScene.name + "Spawn.prefab");
        if (null != prefabSearch)
            wizard.spawnPrefab = prefabSearch.transform;
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
            Scene mirrorScene = InitMirrorScene();
            InvertBoxColliders(MirrorRootObjects(mirrorScene));
            SaveMirrorScene(mirrorScene);
        }
    }
    private Scene InitMirrorScene()
    {
        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
        string currentSceneFullPath = Application.dataPath + source.Substring(source.IndexOfAny("/\\".ToCharArray()));
        string mirrorSceneFullPath = Application.dataPath + destination.Substring(destination.IndexOfAny("/\\".ToCharArray()));
        System.IO.File.Copy(currentSceneFullPath, mirrorSceneFullPath, true);
        EditorSceneManager.OpenScene(destination, OpenSceneMode.Single);
        return SceneManager.GetActiveScene();
    }
    private GameObject[] MirrorRootObjects(Scene scene)
    {
        Transform go = new GameObject("TEMPMIRRORROOT").transform;
        go.parent = null;
        go.localPosition = spawnPrefab.localPosition;
        go.localRotation = spawnPrefab.localRotation;
        go.localScale = spawnPrefab.localScale;
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
        DestroyImmediate(go.gameObject);
        return scene.GetRootGameObjects();
    }
    private Queue<Transform> GetTransforms(GameObject[] gameObjects)
    {
        Queue<Transform> transforms = new Queue<Transform>();
        foreach (GameObject rootObject in gameObjects)
            transforms.Enqueue(rootObject.transform);
        return transforms;
    }
    private List<BoxCollider> GetBoxColliders(GameObject[] gameObjects)
    {
        Queue<Transform> transforms = GetTransforms(gameObjects);
        List<BoxCollider> boxColliders = new List<BoxCollider>();
        Transform tmpT = null;
        BoxCollider tmpBc = null;
        while (transforms.Count > 0)
        {
            tmpT = transforms.Dequeue();
            for (int i = 0; i < tmpT.childCount; ++i)
                transforms.Enqueue(tmpT.GetChild(i));
            if (null != (tmpBc = tmpT.GetComponent<BoxCollider>()))
                boxColliders.Add(tmpBc);
        }
        return boxColliders;
    }
    private void InvertBoxColliders(GameObject[] gameObjects)
    {
        List<BoxCollider> boxColliders = GetBoxColliders(gameObjects);
        Vector3 tmpV3 = Vector3.zero;
        foreach (BoxCollider boxCollider in boxColliders)
        {
            tmpV3 = boxCollider.size;
            tmpV3.x = -tmpV3.x;
            boxCollider.size = tmpV3;
        }
    }
    private void SaveMirrorScene(Scene scene)
    {
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.OpenScene(source, OpenSceneMode.Single));
        EditorSceneManager.SaveOpenScenes();
    }
}