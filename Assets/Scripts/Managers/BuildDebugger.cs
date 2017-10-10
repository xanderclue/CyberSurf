public class BuildDebugger : UnityEngine.MonoBehaviour
{
    private const int cstIntMaxNumLines = 20;
    private const int cstIntMaxLineLength = 45;
    private static System.Collections.Generic.List<string> stcStrlistLines = null;
    private static UnityEngine.GameObject stcGobjTextObject = null;
    private static TMPro.TextMeshProUGUI stcCompTextmesh = null;
    private static uint stcUintLineCounter = 0u;
    private static bool stcBoolLinesSync = false;
    private static bool stcBoolDebuggerInited = false;
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
            (UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton2) &&
            UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton0) &&
            UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.JoystickButton4)))
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
            (UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton2) &&
            UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton0) &&
            UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.JoystickButton5)))
            TestFunc(-1);
    }
    private void TestFunc(int pIntId = 0)
    {
        switch (pIntId)
        {
            case 0:
                {
                    UnityEngine.Debug.Log("Mirroring");
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
                break;
            default:
                break;
        }
    }
    private static void GetLog(string pStrLogMessage, string pStrStackTrace, UnityEngine.LogType pEnmLogType)
    {
        WriteLine(pEnmLogType.ToString() + " << " + pStrLogMessage);
    }
    public static void InitDebugger()
    {
        if (!stcBoolDebuggerInited)
        {
            if (null == stcStrlistLines)
                stcStrlistLines = new System.Collections.Generic.List<string>();
            stcBoolDebuggerInited = true;
            UnityEngine.Application.logMessageReceivedThreaded += GetLog;
        }
    }
    private void OnApplicationQuit()
    {
        if (stcBoolDebuggerInited)
        {
            UnityEngine.Application.logMessageReceivedThreaded -= GetLog;
            stcBoolDebuggerInited = false;
        }
    }
    public static string GetHierarchyName(UnityEngine.GameObject pGobjGameObject)
    {
        string lStrRetVal = "";
        for (UnityEngine.Transform lTfmTransform = pGobjGameObject.transform; null != lTfmTransform; lTfmTransform = lTfmTransform.parent)
            lStrRetVal = lTfmTransform.gameObject.name + "." + lStrRetVal;
        return lStrRetVal.Substring(0, lStrRetVal.Length - 1);
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