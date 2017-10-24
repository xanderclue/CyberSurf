#define DEBUGGER
public class BuildDebugger : UnityEngine.MonoBehaviour
{
    public static string TimeStamp { get { return System.DateTime.Now.ToString("yyyyMMddHHmmssfff"); } }
    public static string GetHierarchyName(UnityEngine.GameObject pGobjGameObject)
    {
#if DEBUGGER
        string lStrRetVal = "";
        for (UnityEngine.Transform lTfmTransform = pGobjGameObject.transform; null != lTfmTransform; lTfmTransform = lTfmTransform.parent)
            lStrRetVal = lTfmTransform.gameObject.name + "." + lStrRetVal;
        return lStrRetVal.Substring(0, lStrRetVal.Length - 1);
#else
        return pGobjGameObject.name;
#endif
    }
#if DEBUGGER
    private static string LogFilePath(string pStrTimeStamp)
    {
        return UnityEngine.Application.persistentDataPath + "/log" + pStrTimeStamp.Substring(0, 8) + ".txt";
    }
#endif
    public static void InitDebugger()
    {
#if DEBUGGER
        if (!stcBoolDebuggerInited)
        {
            if (null == stcStrlistLines)
                stcStrlistLines = new System.Collections.Generic.List<string>();
            string lStrTimeStamp = TimeStamp;
            string logpath = LogFilePath(lStrTimeStamp);
            string logError = "Unknown Error";
            try { stcSwWriter = new System.IO.StreamWriter(logpath, true); } catch (System.Exception e) { stcSwWriter = null; logError = e.Message; }
            if (null != stcSwWriter)
            {
                stcBoolFileOpen = true;
                stcSwWriter.Write("<BEGIN>\n\n");
            }
            else
                UnityEngine.Debug.LogWarning("Failed to open log file: (" + logpath + ") [" + logError + "]");
            stcBoolDebuggerInited = true;
            WriteLine("Log Startup: " + lStrTimeStamp);
            WriteToErrorLog("INIT LOG", logpath + "\n", UnityEngine.LogType.Log, lStrTimeStamp);
            UnityEngine.Application.logMessageReceived += GetLog;
        }
#endif
    }
#if DEBUGGER
    private const int cstIntMaxNumLines = 20;
    private const int cstIntMaxLineLength = 45;
    private static System.Collections.Generic.List<string> stcStrlistLines = null;
    private static System.IO.StreamWriter stcSwWriter;
    private static UnityEngine.GameObject stcGobjTextObject = null;
    private static TMPro.TextMeshProUGUI stcCompTextmesh = null;
    private static ulong stcUlongLineCounter = 0ul, stcUlongFrameCounter = 0ul;
    private static bool stcBoolDebuggerInited = false;
    private static bool stcBoolFileOpen = false;
    private const string DozDig = "0123456789xe";
    public static string Dozenal(ulong i)
    {
        if (0ul == i)
            return "0";
        string rv = "";
        while (0ul != i)
        {
            rv = DozDig[(int)(i % 12ul)] + rv;
            i /= 12ul;
        }
        return rv;
    }
    private static void GetLog(string pStrLogMessage, string pStrStackTrace, UnityEngine.LogType pEnmLogType)
    {
        string lStrTimeStamp = TimeStamp;
        WriteLine(pEnmLogType.ToString() + " << " + pStrLogMessage);
        if (UnityEngine.LogType.Log != pEnmLogType)
            WriteToErrorLog(pStrLogMessage, pStrStackTrace, pEnmLogType, lStrTimeStamp);
    }
    private static void WriteToErrorLog(string pStrLogMessage, string pStrStackTrace, UnityEngine.LogType pEnmLogType, string pStrTimeStamp)
    {
        if (stcBoolFileOpen)
        {
            stcSwWriter.Write("[!!" + pEnmLogType.ToString().ToUpper() + "!!]\n" +
                "Time: " + pStrTimeStamp +
                " @" + Dozenal(stcUlongFrameCounter) +
                "\nLog Message: \"" + pStrLogMessage + "\"\n");
            if (null == pStrStackTrace || "" == pStrStackTrace)
                stcSwWriter.Write("NO STACK TRACE\n\n");
            else
                stcSwWriter.Write("Stack Trace:\n" + pStrStackTrace + "\n");
        }
    }
    private static bool WriteLine(string pStrLine)
    {
        if (null != pStrLine && pStrLine.Length > 0)
        {
            string[] lStrarrSplitLines = pStrLine.Split('\n');
            if (lStrarrSplitLines.Length > 1)
            {
                foreach (string lStrSplitLine in lStrarrSplitLines)
                    if (WriteLine(lStrSplitLine))
                        --stcUlongLineCounter;
                ++stcUlongLineCounter;
                return true;
            }
        }
        else
            return false;
        if (pStrLine.Length > cstIntMaxLineLength)
        {
            WriteLine(pStrLine.Substring(0, cstIntMaxLineLength));
            --stcUlongLineCounter;
            return WriteLine(pStrLine.Substring(cstIntMaxLineLength));
        }
        if (null == stcStrlistLines)
            stcStrlistLines = new System.Collections.Generic.List<string>();
        pStrLine = Dozenal(stcUlongLineCounter) + ": " + pStrLine;
        ++stcUlongLineCounter;
        stcStrlistLines.Add(pStrLine);
        if (stcStrlistLines.Count > cstIntMaxNumLines)
            stcStrlistLines.RemoveAt(0);
        return true;
    }
    private void Awake()
    {
        InitDebugger();
    }
    private void Start()
    {
        stcCompTextmesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (null != stcCompTextmesh)
        {
            stcGobjTextObject = stcCompTextmesh.gameObject;
            stcGobjTextObject.SetActive(false);
        }
        else enabled = false;
    }
    private void OnApplicationQuit()
    {
        if (stcBoolDebuggerInited)
        {
            UnityEngine.Application.logMessageReceived -= GetLog;
            if (stcBoolFileOpen)
            {
                stcSwWriter.Write("<END>\n\n\n");
                stcSwWriter.Close();
            }
        }
    }
#endif
    private void Update()
    {
#if DEBUGGER
        ++stcUlongFrameCounter;
        string lStrTemp = "";
        foreach (string lStrLine in stcStrlistLines)
            lStrTemp += lStrLine + "\n";
        stcCompTextmesh.SetText(lStrTemp);
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.BackQuote) ||
            (UnityEngine.Input.GetKey(KeyInputManager.XBOX_X) &&
            UnityEngine.Input.GetKey(KeyInputManager.XBOX_A) &&
            UnityEngine.Input.GetKeyDown(KeyInputManager.XBOX_LB)))
            stcGobjTextObject.SetActive(!stcGobjTextObject.activeSelf);
#endif
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F2))
            UnityEngine.ScreenCapture.CaptureScreenshot(UnityEngine.Application.persistentDataPath + "/Cybersurf_" + TimeStamp + ".png");
    }
}
public class LabelOverride : UnityEngine.PropertyAttribute
{
    public string mStrLabel;
    public LabelOverride(string pStrLabel) { mStrLabel = pStrLabel; }
#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(LabelOverride))]
    public class ThisPropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect pSctPosRect, UnityEditor.SerializedProperty pClProp, UnityEngine.GUIContent pClLabel)
        {
            pClLabel.text = ((LabelOverride)attribute).mStrLabel;
            UnityEditor.EditorGUI.PropertyField(pSctPosRect, pClProp, pClLabel);
        }
    }
#endif
}
#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class SteamAppId
{
    public const int cstIntSteamAppId = 735810;
    static SteamAppId()
    {
        System.IO.StreamWriter lSwSteamAppIdFile = new System.IO.StreamWriter(UnityEngine.Application.dataPath + "/../steam_appid.txt", false);
        lSwSteamAppIdFile.Write(cstIntSteamAppId);
        lSwSteamAppIdFile.Close();
    }
}
#endif