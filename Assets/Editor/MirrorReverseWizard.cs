using UnityEditor;
using UnityEngine.SceneManagement;
using MirrorReverseHelperClasses;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
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
        {
            CreateMirrorReverse mirrorReverse = new CreateMirrorReverse { sceneName = scene };
            mirrorReverse.CreateScenes();
        }
        EditorSceneManager.OpenScene(originalScene, OpenSceneMode.Single);
    }
    private void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        if (null == scenes || scenes.Length < 1)
            errorString = "No scenes selected";
        else
            foreach (string scene in scenes)
            {
                if (null == scene || "" == scene)
                {
                    errorString = "Empty scene name";
                    break;
                }
                string originalScenePath = string.Format("Assets/Scenes/{0}.unity", scene);
                string originalSpawnPath = string.Format("Assets/Prefabs/SpawnPoints/{0}Spawn.prefab", scene);
                if (!File.Exists(originalScenePath.GetFullPath()))
                {
                    errorString = string.Format("Cannot find {0} scene @ {1}", scene, originalScenePath);
                    break;
                }
                else if (!File.Exists(originalSpawnPath.GetFullPath()))
                {
                    errorString = string.Format("Cannot find {0} spawn point @ {1}", scene, originalSpawnPath);
                    break;
                }
            }
        if ("" == errorString)
            isValid = true;
    }
}