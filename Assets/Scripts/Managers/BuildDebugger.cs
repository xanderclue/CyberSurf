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
            stcBoolDebuggerInited = true;
            string lStrTimeStamp = TimeStamp;
            WriteLine("Startup: " + lStrTimeStamp);
            WriteToErrorLog("STARTUP", UnityEngine.Application.persistentDataPath + "\n", UnityEngine.LogType.Log, lStrTimeStamp);
            UnityEngine.Application.logMessageReceivedThreaded += GetLog;
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
    private static bool stcBoolLinesSync = false;
    private static bool stcBoolDebuggerInited = false;
    private static bool stcBoolErrorLogInited = false;
    private static string TimeStamp { get { return System.DateTime.Now.ToString("yyyyMMddHHmmssfff"); } }
    private static void TestFunc()
    {
        UnityEngine.GameObject[] lGobjarrRootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        System.Collections.Generic.List<UnityEngine.GameObject> lGobjlistRootObjects = new System.Collections.Generic.List<UnityEngine.GameObject>();
        foreach (UnityEngine.GameObject lGobjRootObject in lGobjarrRootObjects)
            lGobjlistRootObjects.Add(lGobjRootObject);
        UnityEngine.GameObject lGobjMirrorRoot = new UnityEngine.GameObject("MIRRORROOT");
        lGobjMirrorRoot.transform.position = GameManager.player.transform.position;
        lGobjMirrorRoot.transform.rotation = GameManager.player.transform.rotation;
        foreach (UnityEngine.GameObject lGobjRootObject in lGobjlistRootObjects)
            lGobjRootObject.transform.parent = lGobjMirrorRoot.transform;
        UnityEngine.Vector3 lVecTempLocalScale = lGobjMirrorRoot.transform.localScale;
        lVecTempLocalScale.x = -lVecTempLocalScale.x;
        lGobjMirrorRoot.transform.localScale = lVecTempLocalScale;
        foreach (UnityEngine.GameObject lGobjRootObject in lGobjlistRootObjects)
            lGobjRootObject.transform.parent = null;
        Destroy(lGobjMirrorRoot);
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
        try
        {
            if (!stcBoolErrorLogInited)
            {
                stcSwWriter = new System.IO.StreamWriter(UnityEngine.Application.persistentDataPath + "/errorLog" + pStrTimeStamp.Substring(0, 8) + ".txt", true);
                stcSwWriter.Write("<BEGIN>\n\n");
                stcBoolErrorLogInited = true;
            }
            stcSwWriter.Write("[!!" + pEnmLogType.ToString().ToUpper() + "!!]\n" +
                "Time: " + pStrTimeStamp +
                "\nLog Message: \"" + pStrLogMessage + "\"\n");
            if (null != pStrStackTrace && "" != pStrStackTrace)
                stcSwWriter.Write("Stack Trace:\n" + pStrStackTrace + "\n");
        }
        catch (System.Exception e) { WriteLine("kánótlógerRtufAil: " + e.Message); }
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
        while (stcBoolLinesSync) if (!stcBoolLinesSync) break;
        stcBoolLinesSync = true;
        stcStrlistLines.Add(pStrLine);
        if (stcStrlistLines.Count > cstIntMaxNumLines)
            stcStrlistLines.RemoveAt(0);
        stcBoolLinesSync = false;
        return true;
    }
    private void Awake()
    {
        InitDebugger();
    }
    private void Update()
    {
        if (null == stcGobjTextObject)
        {
            stcCompTextmesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (null == stcCompTextmesh)
                return;
            stcGobjTextObject = stcCompTextmesh.gameObject;
            stcGobjTextObject.SetActive(false);
        }
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.BackQuote) ||
            (UnityEngine.Input.GetKey(KeyInputManager.XBOX_X) &&
            UnityEngine.Input.GetKey(KeyInputManager.XBOX_A) &&
            UnityEngine.Input.GetKeyDown(KeyInputManager.XBOX_LB)))
            stcGobjTextObject.SetActive(!stcGobjTextObject.activeSelf);
        string lStrTemp = "";
        while (stcBoolLinesSync) if (!stcBoolLinesSync) break;
        stcBoolLinesSync = true;
        foreach (string lStrLine in stcStrlistLines)
            lStrTemp += lStrLine + "\n";
        stcBoolLinesSync = false;
        stcCompTextmesh.SetText(lStrTemp);
        stcCompTextmesh.ForceMeshUpdate();
        if ((UnityEngine.Input.GetKey(UnityEngine.KeyCode.K) &&
            UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.T)) ||
            (UnityEngine.Input.GetKey(KeyInputManager.XBOX_X) &&
            UnityEngine.Input.GetKey(KeyInputManager.XBOX_A) &&
            UnityEngine.Input.GetKeyDown(KeyInputManager.XBOX_RB)))
            TestFunc();
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F2))
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftShift) && UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightShift))
                UnityEngine.ScreenCapture.CaptureScreenshot(UnityEngine.Application.persistentDataPath + "/Cybersurf_" + TimeStamp + "_Triple.png", 3);
            else if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftShift) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightShift))
                UnityEngine.ScreenCapture.CaptureScreenshot(UnityEngine.Application.persistentDataPath + "/Cybersurf_" + TimeStamp + "_Double.png", 2);
            else
                UnityEngine.ScreenCapture.CaptureScreenshot(UnityEngine.Application.persistentDataPath + "/Cybersurf_" + TimeStamp + ".png");
    }
    private void OnApplicationQuit()
    {
        if (stcBoolDebuggerInited) UnityEngine.Application.logMessageReceivedThreaded -= GetLog;
        if (stcBoolErrorLogInited) try { stcSwWriter.Write("<END>\n\n\n"); stcSwWriter.Close(); } catch { }
    }
#endif
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