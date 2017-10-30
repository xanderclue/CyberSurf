using UnityEngine;
using UnityEditor;
using System.IO;
[InitializeOnLoad]
public class RedistInstall
{
    static RedistInstall()
    {
        CopyFile("Assets/Plugins/Steamworks.NET/redist", "steam_appid.txt", false);
#if UNITY_EDITOR_WIN && (UNITY_4_7 || UNITY_5_0)
#if UNITY_EDITOR_64
        CopyFile("Assets/Plugins/x86_64", "steam_api64.dll", true);
#else
        CopyFile("Assets/Plugins/x86", "steam_api.dll", true);
#endif
#endif
#if UNITY_5 || UNITY_2017
#if !DISABLEPLATFORMSETTINGS
        SetPlatformSettings();
#endif
#endif
    }
    private static void CopyFile(string path, string filename, bool bCheckDifference)
    {
        string strCWD = Directory.GetCurrentDirectory();
        string strSource = Path.Combine(Path.Combine(strCWD, path), filename);
        string strDest = Path.Combine(strCWD, filename);
        if (File.Exists(strSource))
        {
            if (File.Exists(strDest))
            {
                if (!bCheckDifference)
                    return;
                if (File.GetLastWriteTime(strSource) == File.GetLastWriteTime(strDest))
                    if (new FileInfo(strSource).Length == new FileInfo(strDest).Length)
                        return;
                Debug.Log(string.Format("[Steamworks.NET] {0} in the project root differs from the Steamworks.NET redistributable. Updating.... Please relaunch Unity.", filename));
            }
            else
                Debug.Log(string.Format("[Steamworks.NET] {0} is not present in the project root. Copying...", filename));
            File.Copy(strSource, strDest, true);
            File.SetAttributes(strDest, File.GetAttributes(strDest) & ~FileAttributes.ReadOnly);
            if (File.Exists(strDest))
                Debug.Log(string.Format("[Steamworks.NET] Successfully copied {0} into the project root. Please relaunch Unity.", filename));
            else
                Debug.LogWarning(string.Format("[Steamworks.NET] Could not copy {0} into the project root. File.Copy() Failed. Please copy {0} into the project root manually.", Path.Combine(path, filename)));
        }
        else
            Debug.LogWarning(string.Format("[Steamworks.NET] Could not copy {0} into the project root. {0} could not be found in '{1}'. Place {0} from the Steamworks SDK in the project root manually.", filename, Path.Combine(strCWD, path)));
    }
#if UNITY_5 || UNITY_2017
    private static void SetPlatformSettings()
    {
        foreach (PluginImporter plugin in PluginImporter.GetAllImporters())
        {
            if (null == plugin)
                continue;
            if (Path.IsPathRooted(plugin.assetPath))
                continue;
            bool didUpdate = false;
            switch (Path.GetFileName(plugin.assetPath))
            {
                case "CSteamworks.bundle":
                    didUpdate |= ResetPluginSettings(plugin, "AnyCPU", "OSX");
                    didUpdate |= SetCompatibleWithOSX(plugin);
                    break;
                case "libCSteamworks.so":
                case "libsteam_api.so":
                    if (plugin.assetPath.Contains("x86_64"))
                    {
                        didUpdate |= ResetPluginSettings(plugin, "x86_64", "Linux");
                        didUpdate |= SetCompatibleWithLinux(plugin, BuildTarget.StandaloneLinux64);
                    }
                    else
                    {
                        didUpdate |= ResetPluginSettings(plugin, "x86", "Linux");
                        didUpdate |= SetCompatibleWithLinux(plugin, BuildTarget.StandaloneLinux);
                    }
                    break;
                case "CSteamworks.dll":
                    if (plugin.assetPath.Contains("x86_64"))
                    {
                        didUpdate |= ResetPluginSettings(plugin, "x86_64", "Windows");
                        didUpdate |= SetCompatibleWithWindows(plugin, BuildTarget.StandaloneWindows64);
                    }
                    else
                    {
                        didUpdate |= ResetPluginSettings(plugin, "x86", "Windows");
                        didUpdate |= SetCompatibleWithWindows(plugin, BuildTarget.StandaloneWindows);
                    }
                    break;
                case "steam_api.dll":
                case "steam_api64.dll":
                    if (plugin.assetPath.Contains("x86_64"))
                    {
                        didUpdate |= ResetPluginSettings(plugin, "x86_64", "Windows");
#if UNITY_5_3_OR_NEWER
                        didUpdate |= SetCompatibleWithWindows(plugin, BuildTarget.StandaloneWindows64);
#endif
                    }
                    else
                    {
                        didUpdate |= ResetPluginSettings(plugin, "x86", "Windows");
#if UNITY_5_3_OR_NEWER
                        didUpdate |= SetCompatibleWithWindows(plugin, BuildTarget.StandaloneWindows);
#endif
                    }
#if !UNITY_5_3_OR_NEWER
                    didUpdate |= SetCompatibleWithEditor(plugin);
#endif
                    break;
            }
            if (didUpdate)
                plugin.SaveAndReimport();
        }
    }
    private static bool ResetPluginSettings(PluginImporter plugin, string CPU, string OS)
    {
        bool didUpdate = false;
        if (plugin.GetCompatibleWithAnyPlatform())
        {
            plugin.SetCompatibleWithAnyPlatform(false);
            didUpdate = true;
        }
        if (!plugin.GetCompatibleWithEditor())
        {
            plugin.SetCompatibleWithEditor(true);
            didUpdate = true;
        }
        if (plugin.GetEditorData("CPU") != CPU)
        {
            plugin.SetEditorData("CPU", CPU);
            didUpdate = true;
        }
        if (plugin.GetEditorData("OS") != OS)
        {
            plugin.SetEditorData("OS", OS);
            didUpdate = true;
        }
        return didUpdate;
    }
    private static bool SetCompatibleWithOSX(PluginImporter plugin)
    {
        bool didUpdate = false;
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel, true);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel64, true);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXUniversal, true);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux64, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinuxUniversal, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows64, false);
        return didUpdate;
    }
    private static bool SetCompatibleWithLinux(PluginImporter plugin, BuildTarget platform)
    {
        bool didUpdate = false;
        if (platform == BuildTarget.StandaloneLinux)
        {
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux, true);
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux64, false);
        }
        else
        {
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux, false);
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux64, true);
        }
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinuxUniversal, true);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel64, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXUniversal, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows64, false);
        return didUpdate;
    }
    private static bool SetCompatibleWithWindows(PluginImporter plugin, BuildTarget platform)
    {
        bool didUpdate = false;
        if (platform == BuildTarget.StandaloneWindows)
        {
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows, true);
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows64, false);
        }
        else
        {
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows, false);
            didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows64, true);
        }
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux64, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinuxUniversal, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel64, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXUniversal, false);
        return didUpdate;
    }
    private static bool SetCompatibleWithEditor(PluginImporter plugin)
    {
        bool didUpdate = false;
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux64, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinux, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneLinuxUniversal, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXIntel64, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSXUniversal, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows64, false);
        return didUpdate;
    }
    private static bool SetCompatibleWithPlatform(PluginImporter plugin, BuildTarget platform, bool enable)
    {
        if (plugin.GetCompatibleWithPlatform(platform) == enable)
            return false;
        plugin.SetCompatibleWithPlatform(platform, enable);
        return true;
    }
#endif
}