using UnityEditor;
using UnityEngine.SceneManagement;
using MirrorReverseHelperClasses;
using System.IO;
using UnityEditor.SceneManagement;
public class CreateMirrorReverseMultiple : ScriptableWizard
{
    public string[] scenes = null;
    [MenuItem("Cybersurf Tools/Create Multiple Mirror and Reverse...")]
    private static void ProcessMirrorReverse()
    {
        CreateMirrorReverseMultiple wizard = DisplayWizard<CreateMirrorReverseMultiple>("Create Multiple Mirror and Reverse Scenes", "Create Scenes", "Get Default Levels");
        wizard.scenes = new string[1];
        wizard.scenes[0] = SceneManager.GetActiveScene().name;
        wizard.OnWizardUpdate();
    }
    private void OnWizardOtherButton()
    {
        scenes = new string[3];
        scenes[0] = "Canyon";
        scenes[1] = "MultiEnvironment";
        scenes[2] = "BackyardRacetrack";
        OnWizardUpdate();
    }
    private void OnWizardCreate()
    {
        string originalScene = SceneManager.GetActiveScene().path;
        foreach (string scene in scenes)
        {
            CreateMirrorReverse wizard = CreateInstance<CreateMirrorReverse>();
            wizard.sceneName = scene;
            wizard.OnWizardCreate();
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