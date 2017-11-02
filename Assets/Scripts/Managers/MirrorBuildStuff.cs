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
        if (SceneManager.GetActiveScene().isDirty)
            EditorSceneManager.SaveOpenScenes();
        if (null == spawnPrefab)
            return;
        string currentSceneFullPath = Application.dataPath + source.Substring(source.IndexOfAny("/\\".ToCharArray()));
        string mirrorSceneFullPath = Application.dataPath + destination.Substring(destination.IndexOfAny("/\\".ToCharArray()));
        System.IO.File.Copy(currentSceneFullPath, mirrorSceneFullPath, true);
        EditorSceneManager.OpenScene(destination, OpenSceneMode.Single);
        Scene mirrorScene = SceneManager.GetActiveScene();
        Transform go = new GameObject("TEMPMIRRORROOT").transform;
        go.parent = null;
        go.localPosition = spawnPrefab.localPosition;
        go.localRotation = spawnPrefab.localRotation;
        go.localScale = spawnPrefab.localScale;
        mirrorScene.GetRootGameObjects();
        List<Transform> rootTransforms = new List<Transform>();
        GameObject[] rootObjects = mirrorScene.GetRootGameObjects();
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
        EditorSceneManager.MarkSceneDirty(mirrorScene);
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(source, OpenSceneMode.Single);
    }
}