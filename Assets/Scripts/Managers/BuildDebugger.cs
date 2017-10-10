public class BuildDebugger : UnityEngine.MonoBehaviour
{
    private static System.Collections.Generic.List<string> lines = null;
    private static UnityEngine.GameObject textObject = null;
    private static uint lineCounter = 0u;
    private const int maxLines = 20;
    private const int maxLength = 45;
    private static bool linesSync = false;
    private static TMPro.TextMeshProUGUI textmesh = null;
    private const string LOG_START = "BuildDebugger: \"";
    private static bool WriteLine(string line, bool writeToUnity = true)
    {
        if (null != line && line.Length > 0)
        {
            string[] splitLines = line.Split('\n');
            if (splitLines.Length > 1)
            {
                if (writeToUnity)
                    UnityEngine.Debug.Log(LOG_START + line + "\"");
                foreach (string splitLine in splitLines)
                {
                    if (WriteLine(splitLine, false))
                        --lineCounter;
                }
                ++lineCounter;
                return true;
            }
        }
        else
            return false;
        if (line.Length > maxLength)
        {
            if (writeToUnity)
                UnityEngine.Debug.Log(LOG_START + line + "\"");
            WriteLine(line.Substring(0, maxLength), false);
            --lineCounter;
            return WriteLine(line.Substring(maxLength), false);
        }
        if (writeToUnity)
            UnityEngine.Debug.Log(LOG_START + line + "\"");
        if (null == lines)
            lines = new System.Collections.Generic.List<string>();
        line = lineCounter.ToString() + ": " + line;
        ++lineCounter;
        while (linesSync) if (!linesSync) break;
        linesSync = true;
        lines.Add(line);
        if (lines.Count > maxLines)
            lines.RemoveAt(0);
        linesSync = false;
        return true;
    }
    private void Awake()
    {
        if (null == lines)
            lines = new System.Collections.Generic.List<string>();
    }
    private void Update()
    {
        if (null == textObject)
        {
            textmesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (null == textmesh)
                return;
            textObject = textmesh.gameObject;
            textObject.SetActive(false);
        }
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.BackQuote) ||
            (UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton2) &&
            UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton0) &&
            UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.JoystickButton4)))
        {
            textObject.SetActive(!textObject.activeSelf);
            WriteLine("Build Debugger " + (textObject.activeSelf ? "SHOWN" : "HIDDEN"));
        }
        string tmstr = "";
        while (linesSync) if (!linesSync) break;
        linesSync = true;
        foreach (string str in lines)
            tmstr += str + "\n";
        linesSync = false;
        textmesh.SetText(tmstr);
        textmesh.ForceMeshUpdate();
        if ((UnityEngine.Input.GetKey(UnityEngine.KeyCode.K) &&
            UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.T)) ||
            (UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton2) &&
            UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton0) &&
            UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.JoystickButton5)))
            TestFunc(-1);
    }
    private void TestFunc(int id = 0)
    {
        switch (id)
        {
            case 0:
                {
                    WriteLine("Mirroring");
                    UnityEngine.GameObject[] objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                    System.Collections.Generic.List<UnityEngine.GameObject> objsList = new System.Collections.Generic.List<UnityEngine.GameObject>();
                    foreach (UnityEngine.GameObject gObj in objs)
                        objsList.Add(gObj);
                    UnityEngine.GameObject go = new UnityEngine.GameObject("MIRRORROOT");
                    go.transform.position = GameManager.player.transform.position;
                    go.transform.rotation = GameManager.player.transform.rotation;
                    foreach (UnityEngine.GameObject gObj in objsList)
                        gObj.transform.parent = go.transform;
                    UnityEngine.Vector3 rScale = go.transform.localScale;
                    rScale.x = -rScale.x;
                    go.transform.localScale = rScale;
                    foreach (UnityEngine.GameObject gObj in objsList)
                        gObj.transform.parent = null;
                    Destroy(go);
                }
                break;
            default:
                break;
        }
    }
    private static bool debuggerInited = false;
    private static void GetLog(string logMessage, string stackTrace, UnityEngine.LogType logType)
    {
        if (!logMessage.StartsWith(LOG_START))
            WriteLine(logType.ToString() + " << " + logMessage, false);
    }
    public static void InitDebugger()
    {
        if (!debuggerInited)
        {
            debuggerInited = true;
            UnityEngine.Application.logMessageReceivedThreaded += GetLog;
        }
    }
    private void OnApplicationQuit()
    {
        if (debuggerInited)
        {
            UnityEngine.Application.logMessageReceivedThreaded -= GetLog;
            debuggerInited = false;
        }
    }
    private void OnEnable()
    {
        InitDebugger();
    }
    public static string GetHierarchyName(UnityEngine.GameObject gObj)
    {
        string outStr = "";
        for (UnityEngine.Transform tr = gObj.transform; null != tr; tr = tr.parent)
            outStr = tr.gameObject.name + "." + outStr;
        return outStr.Substring(0, outStr.Length - 1);
    }
}
public class LabelOverride : UnityEngine.PropertyAttribute
{
    public string m_label;
    public LabelOverride(string _label) { m_label = _label; }
#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(LabelOverride))]
    public class ThisPropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(UnityEngine.Rect _pos, UnityEditor.SerializedProperty _prop, UnityEngine.GUIContent _label)
        {
            _label.text = ((LabelOverride)attribute).m_label;
            UnityEditor.EditorGUI.PropertyField(_pos, _prop, _label);
        }
    }
#endif
}