public class BuildDebugger : UnityEngine.MonoBehaviour
{
    private static System.Collections.Generic.List<string> lines = null;
    private static UnityEngine.GameObject textObject = null;
    private static uint lineCounter = 0u;
    private const int maxLines = 20;
    private static bool linesSync = false;
    private static TMPro.TextMeshProUGUI textmesh = null;
    public static void WriteLine(string line)
    {
        if (null == lines)
            return;
        line += "\n";
        line = lineCounter.ToString() + ": " + line.Substring(0, line.IndexOf('\n'));
        ++lineCounter;
        while (linesSync) if (!linesSync) break;
        linesSync = true;
        lines.Add(line);
        if (lines.Count > maxLines)
            lines.RemoveAt(0);
        linesSync = false;
        UnityEngine.Debug.Log("BuildDebugger: Line added: \"" + line + "\"");
    }
    private void Awake()
    {
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
            UnityEngine.Input.GetKey(UnityEngine.KeyCode.JoystickButton3) &&
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
    }
}