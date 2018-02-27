using UnityEditor;
using System.IO;
[InitializeOnLoad]
public class RedistInstall
{
    static RedistInstall()
    {
        string strCWD = Directory.GetCurrentDirectory();
        string strSource = Path.Combine(strCWD, "Assets/Plugins/Steamworks.NET/redist/steam_appid.txt"), strDest = Path.Combine(strCWD, "steam_appid.txt");
        bool didUpdate;
        if (File.Exists(strSource) && !File.Exists(strDest))
        {
            File.Copy(strSource, strDest, true);
            File.SetAttributes(strDest, File.GetAttributes(strDest) & ~FileAttributes.ReadOnly);
        }
        foreach (PluginImporter plugin in PluginImporter.GetAllImporters())
        {
            if (null != plugin && !Path.IsPathRooted(plugin.assetPath))
            {
                didUpdate = false;
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
                            didUpdate |= SetCompatibleWithWindows(plugin, BuildTarget.StandaloneWindows64);
                        }
                        else
                        {
                            didUpdate |= ResetPluginSettings(plugin, "x86", "Windows");
                            didUpdate |= SetCompatibleWithWindows(plugin, BuildTarget.StandaloneWindows);
                        }
                        break;
                }
                if (didUpdate)
                    plugin.SaveAndReimport();
            }
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
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSX, true);
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
        if (BuildTarget.StandaloneLinux == platform)
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
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSX, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows, false);
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneWindows64, false);
        return didUpdate;
    }
    private static bool SetCompatibleWithWindows(PluginImporter plugin, BuildTarget platform)
    {
        bool didUpdate = false;
        if (BuildTarget.StandaloneWindows == platform)
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
        didUpdate |= SetCompatibleWithPlatform(plugin, BuildTarget.StandaloneOSX, false);
        return didUpdate;
    }
    private static bool SetCompatibleWithPlatform(PluginImporter plugin, BuildTarget platform, bool enable)
    {
        if (plugin.GetCompatibleWithPlatform(platform) == enable)
            return false;
        plugin.SetCompatibleWithPlatform(platform, enable);
        return true;
    }
}