#if UNITY_5_3_OR_NEWER
#define DISABLEREDISTCOPY
#endif
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
public class RedistCopy
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
#if !DISABLEREDISTCOPY
        if (BuildTarget.StandaloneWindows != target && BuildTarget.StandaloneWindows64 != target &&
             BuildTarget.StandaloneOSXIntel != target && BuildTarget.StandaloneOSXIntel64 != target && BuildTarget.StandaloneOSXUniversal != target &&
             BuildTarget.StandaloneLinux != target && BuildTarget.StandaloneLinux64 != target && BuildTarget.StandaloneLinuxUniversal != target)
            return;
        string strProjectName = Path.GetFileNameWithoutExtension(pathToBuiltProject);
        if (BuildTarget.StandaloneWindows64 == target)
            CopyFile("steam_api64.dll", "steam_api64.dll", "Assets/Plugins/x86_64", pathToBuiltProject);
        else if (BuildTarget.StandaloneWindows == target)
            CopyFile("steam_api.dll", "steam_api.dll", "Assets/Plugins/x86", pathToBuiltProject);
        string controllerCfg = Path.Combine(Application.dataPath, "controller.vdf");
        if (File.Exists(controllerCfg))
        {
            string strFileDest = Path.Combine(Path.Combine(Path.GetDirectoryName(pathToBuiltProject), strProjectName + (
                (target == BuildTarget.StandaloneOSXIntel || target == BuildTarget.StandaloneOSXIntel64 || target == BuildTarget.StandaloneOSXUniversal) ?
                ".app/Contents" : "_Data"
                )), "controller.vdf");
            File.Copy(controllerCfg, strFileDest);
            File.SetAttributes(strFileDest, File.GetAttributes(strFileDest) & ~FileAttributes.ReadOnly);
            if (!File.Exists(strFileDest))
                Debug.LogWarning("[Steamworks.NET] Could not copy controller.vdf into the built project. File.Copy() Failed. Place controller.vdf from the Steamworks SDK in the output dir manually.");
        }
#endif
    }
    private static void CopyFile(string inFileName, string outFileName, string pathToInFile, string pathToBuiltProject)
    {
        string inFilePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), pathToInFile), inFileName);
        string outFilePath = Path.Combine(Path.GetDirectoryName(pathToBuiltProject), outFileName);
        if (File.Exists(inFilePath))
        {
            if (File.Exists(outFilePath))
                if (File.GetLastWriteTime(inFilePath) == File.GetLastWriteTime(outFilePath))
                    if (new FileInfo(inFilePath).Length == new FileInfo(outFilePath).Length)
                        return;
            File.Copy(inFilePath, outFilePath, true);
            File.SetAttributes(outFilePath, File.GetAttributes(outFilePath) & ~FileAttributes.ReadOnly);
            if (!File.Exists(outFilePath))
                Debug.LogWarning(string.Format("[Steamworks.NET] Could not copy {0} into the built project. File.Copy() Failed. Place {0} from the redist folder into the output dir manually.", inFileName));
        }
        else
            Debug.LogWarning(string.Format("[Steamworks.NET] Could not copy {0} into the project root. {0} could not be found in '{1}'. Place {0} from the redist into the project root manually.", inFileName, pathToInFile));
    }
}