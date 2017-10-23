#define DEBUGGER
public class BuildDebugger : UnityEngine.MonoBehaviour
{
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
    public static void InitDebugger()
    {
#if DEBUGGER
        if (!stcBoolDebuggerInited)
        {
            if (null == stcStrlistLines)
                stcStrlistLines = new System.Collections.Generic.List<string>();
            string lStrTimeStamp = TimeStamp;
            stcSwWriter = new System.IO.StreamWriter(UnityEngine.Application.persistentDataPath + "/errorLog" + lStrTimeStamp.Substring(0, 8) + ".txt", true);
            stcSwWriter.Write("<BEGIN>\n\n");
            stcBoolDebuggerInited = true;
            WriteLine("Log Startup: " + lStrTimeStamp);
            WriteToErrorLog("INIT LOG", UnityEngine.Application.persistentDataPath + "/errorLog" + lStrTimeStamp.Substring(0, 8) + ".txt\n", UnityEngine.LogType.Log, lStrTimeStamp);
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
    private static uint stcUintLineCounter = 0u;
    private static bool stcBoolDebuggerInited = false;
    private static string TimeStamp { get { return System.DateTime.Now.ToString("yyyyMMddHHmmssfff"); } }
    private static void GetLog(string pStrLogMessage, string pStrStackTrace, UnityEngine.LogType pEnmLogType)
    {
        string lStrTimeStamp = TimeStamp;
        WriteLine(pEnmLogType.ToString() + " << " + pStrLogMessage);
        if (UnityEngine.LogType.Log != pEnmLogType)
            WriteToErrorLog(pStrLogMessage, pStrStackTrace, pEnmLogType, lStrTimeStamp);
    }
    private static void WriteToErrorLog(string pStrLogMessage, string pStrStackTrace, UnityEngine.LogType pEnmLogType, string pStrTimeStamp)
    {
        stcSwWriter.Write("[!!" + pEnmLogType.ToString().ToUpper() + "!!]\n" +
            "Time: " + pStrTimeStamp +
            "\nLog Message: \"" + pStrLogMessage + "\"\n");
        if (null == pStrStackTrace || "" == pStrStackTrace)
            stcSwWriter.Write("NO STACK TRACE\n\n");
        else
            stcSwWriter.Write("Stack Trace:\n" + pStrStackTrace + "\n");
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
                        --stcUintLineCounter;
                ++stcUintLineCounter;
                return true;
            }
        }
        else
            return false;
        if (pStrLine.Length > cstIntMaxLineLength)
        {
            WriteLine(pStrLine.Substring(0, cstIntMaxLineLength));
            --stcUintLineCounter;
            return WriteLine(pStrLine.Substring(cstIntMaxLineLength));
        }
        if (null == stcStrlistLines)
            stcStrlistLines = new System.Collections.Generic.List<string>();
        pStrLine = stcUintLineCounter.ToString() + ": " + pStrLine;
        ++stcUintLineCounter;
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
            stcSwWriter.Write("<END>\n\n\n");
            stcSwWriter.Close();
        }
    }
#endif
    private void Update()
    {
#if DEBUGGER
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