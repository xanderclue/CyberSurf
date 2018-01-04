using UnityEditor;
using UnityEditor.SceneManagement;
using MirrorReverseHelperClasses;
using static System.IO.File;
using static UnityEngine.SceneManagement.SceneManager;
using static LevelManager;
public class MirrorReverseWizard : ScriptableWizard
{
    private string[] scenes = null;
    [MenuItem("Cybersurf Tools/Create Mirror and Reverse Levels...")]
    private static void InitMirrorReverseWizard() =>
        DisplayWizard<MirrorReverseWizard>("Create Mirrored and Reversed Levels", "Create Levels").SetupList();
    private void SetupList()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        scenes = new string[(int)Level.NumLevels];
        for (Level i = 0; Level.NumLevels != i; ++i)
            scenes[(int)i] = buildScenes[(int)i + LevelBuildOffset].path.GetSceneName();
        OnWizardUpdate();
    }
    private void OnWizardCreate()
    {
        TransformCleaner.CleanTransforms(true);
        string originalScene = GetActiveScene().path;
        foreach (string scene in scenes)
            new CreateMirrorReverse(scene).CreateScenes();
        EditorSceneManager.OpenScene(originalScene, OpenSceneMode.Single);
        TransformCleaner.CleanTransforms(false);
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
                if (!Exists(originalScenePath.GetFullPath()))
                {
                    errorString = $"Cannot find {scene} scene @ " + originalScenePath;
                    break;
                }
                else if (!Exists(originalSpawnPath.GetFullPath()))
                {
                    errorString = $"Cannot find {scene} spawn point @ " + originalSpawnPath;
                    break;
                }
            }
        if ("" == errorString)
            isValid = true;
    }
}