using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using CreateMirrorReverseExtensions;
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
        GetAllPaths();
        CreateReverseScene();
        CreateMirrorScene();
        CreateReverseMirrorScene();
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
    }
    private void CreateMirrorScene()
    {
        File.Copy(originalScenePath.GetFullPath(), mirrorScenePath.GetFullPath(), true);
        File.Copy(originalSpawnPath.GetFullPath(), mirrorSpawnPath.GetFullPath(), true);
    }
    private void CreateReverseMirrorScene()
    {
        File.Copy(reverseScenePath.GetFullPath(), reverseMirrorScenePath.GetFullPath(), true);
        File.Copy(reverseSpawnPath.GetFullPath(), reverseMirrorSpawnPath.GetFullPath(), true);
    }
}
namespace CreateMirrorReverseExtensions
{
    public static class CreateMirrorReverseExtensions
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
}