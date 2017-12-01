using UnityEngine;
using TMPro;

public class Race_Lap_Ring_Text_Script : MonoBehaviour {


    private TextMeshPro element = null;
    private Lap_Text_script laptext = null;
    private void Start()
    {
        element = GetComponent<TextMeshPro>();
        laptext = GameObject.FindObjectOfType<Lap_Text_script>();
    }

    // Update is called once per frame
    void Update () {

        if (GameModes.Race == GameManager.instance.gameMode.currentMode && laptext.curr_lap != laptext.max_lap)
        {
            element.SetText("Checkpoint");
        }
        else
        {
            element.SetText("Exit");
        }
	}
}
