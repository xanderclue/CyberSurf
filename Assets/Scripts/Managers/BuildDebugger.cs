using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuildDebugger : MonoBehaviour
{
    public static List<string> lines;
    public static GameObject console;
    static uint counter = 0;
    const int maxlines = 20;
    static bool writingLine = false;
    static TextMeshProUGUI textmesh;
    public static void WriteLine(string line)
    {
        line += "\n";
        while (writingLine) if (!writingLine) break;
        writingLine = true;
        lines.Add(counter.ToString() + ": " + line.Substring(0, line.IndexOf('\n') + 1));
        ++counter;
        if (lines.Count > maxlines)
            lines.RemoveAt(0);
        writingLine = false;
    }
    private void Awake()
    {
        lines = new List<string>();
    }
    private void Update()
    {
        if (null == console)
        {
            textmesh = GetComponentInChildren<TextMeshProUGUI>();
            if (null == textmesh)
                return;
            console = textmesh.gameObject;
            console.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.BackQuote) || // Hit ~/` key
            (Input.GetKey(KeyCode.JoystickButton2) &&
            Input.GetKey(KeyCode.JoystickButton3) &&
            Input.GetKeyDown(KeyCode.JoystickButton4))) // Hold down X&Y, and press LB
        {
            console.SetActive(!console.activeSelf);
            WriteLine("Build Debugger " + (console.activeSelf ? "ENABLED" : "DISABLED"));
        }
        while (writingLine) if (!writingLine) break;
        writingLine = true;
        string tmstr = "";
        foreach (string str in lines)
            tmstr += str;
        textmesh.SetText(tmstr);
        writingLine = false;
    }
}