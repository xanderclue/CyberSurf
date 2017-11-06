using UnityEditor;
using UnityEngine.SceneManagement;
using MirrorReverseHelperClasses;
using System.IO;
using UnityEditor.SceneManagement;
public class MirrorReverseWizard : ScriptableWizard
{
    public string[] scenes = null;
    [MenuItem("Cybersurf Tools/Create Mirror and Reverse Levels...")]
    private static void InitMirrorReverseWizard()
    {
        DisplayWizard<MirrorReverseWizard>("Create Mirrored and Reversed Levels", "Create Levels").SetupList();
    }
    private void SetupList()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        scenes = new string[(int)LevelManager.Level.NumLevels];
        for (LevelManager.Level i = 0; LevelManager.Level.NumLevels != i; ++i)
            scenes[(int)i] = buildScenes[(int)i + LevelSelectOptions.LevelBuildOffset].path.GetSceneName();
        OnWizardUpdate();
    }
    private void OnWizardCreate()
    {
        string originalScene = SceneManager.GetActiveScene().path;
        foreach (string scene in scenes)
            new CreateMirrorReverse(scene).CreateScenes();
        EditorSceneManager.OpenScene(originalScene, OpenSceneMode.Single);
        TransformCleaner.CleanTransforms();
    }
    private void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        if (null == scenes || scenes.Length < 1)
            errorString = "Scene list empty";
        else
            foreach (string scene in scenes)
            {
                if (null == scene || "" == scene)
                {
                    errorString = "Empty scene name";
                    break;
                }
                string originalScenePath = $"Assets/Scenes/{scene}.unity";
                string originalSpawnPath = $"Assets/Prefabs/SpawnPoints/{scene}Spawn.prefab";
                if (!File.Exists(originalScenePath.GetFullPath()))
                {
                    errorString = $"Cannot find {scene} scene @ " + originalScenePath;
                    break;
                }
                else if (!File.Exists(originalSpawnPath.GetFullPath()))
                {
                    errorString = $"Cannot find {scene} spawn point @ " + originalSpawnPath;
                    break;
                }
            }
        if ("" == errorString)
            isValid = true;
    }
}