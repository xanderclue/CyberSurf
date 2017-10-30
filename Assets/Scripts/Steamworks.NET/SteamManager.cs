using UnityEngine;
using Steamworks;
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
    private static SteamManager s_instance = null;
    private static SteamManager Instance { get { return s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>(); } }
    private static bool s_EverInialized;
    private bool m_bInitialized;
    public static bool Initialized { get { return Instance.m_bInitialized; } }
    private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
    private static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText);
    }
    private void Awake()
    {
        if (null != s_instance)
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        if (s_EverInialized)
            throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");
        DontDestroyOnLoad(gameObject);
        if (!Packsize.Test())
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        if (!DllCheck.Test())
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        try
        {
            // If Steam is not running or the game wasn't started through Steam, SteamAPI_RestartAppIfNecessary starts the
            // Steam client and also launches this game again if the User owns it. This can act as a rudimentary form of DRM.
            // Once you get a Steam AppID assigned by Valve, you need to replace AppId_t.Invalid with it and
            // remove steam_appid.txt from the game depot. eg: "(AppId_t)480" or "new AppId_t(480)".
            // See the Valve documentation for more information: https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
            if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
            {
                Application.Quit();
                return;
            }
        }
        catch (System.DllNotFoundException e)
        {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);
            Application.Quit();
            return;
        }
        // Initializes the Steamworks API.
        // If this returns false then this indicates one of the following conditions:
        // [*] The Steam client isn't running. A running Steam client is required to provide implementations of the various Steamworks interfaces.
        // [*] The Steam client couldn't determine the App ID of game. If you're running your application from the executable or debugger directly then you must have a [code-inline]steam_appid.txt[/code-inline] in your game directory next to the executable, with your app ID in it and nothing else. Steam will look for this file in the current working directory. If you are running your executable from a different directory you may need to relocate the [code-inline]steam_appid.txt[/code-inline] file.
        // [*] Your application is not running under the same OS user context as the Steam client, such as a different user or administration access level.
        // [*] Ensure that you own a license for the App ID on the currently active Steam account. Your game must show up in your Steam library.
        // [*] Your App ID is not completely set up, i.e. in [code-inline]Release State: Unavailable[/code-inline], or it's missing default packages.
        // Valve's documentation for this is located here:
        // https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
        m_bInitialized = SteamAPI.Init();
        if (!m_bInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
            return;
        }
        s_EverInialized = true;
    }
    private void OnEnable()
    {
        if (null == s_instance)
            s_instance = this;
        if (m_bInitialized && null == m_SteamAPIWarningMessageHook)
        {
            m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
        }
    }
    private void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
            if (m_bInitialized)
                SteamAPI.Shutdown();
        }
    }
    private void Update()
    {
        if (m_bInitialized)
            SteamAPI.RunCallbacks();
    }
}