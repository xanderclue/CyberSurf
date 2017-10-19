using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score_Multi_Script : MonoBehaviour {

    GameManager manager;
    TextMeshProUGUI element;

    void Start()
    {
        manager = GameManager.instance;
        element = gameObject.GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        string textToWrite = " " + manager.scoreScript.score_multiplier + "X Score";
        element.SetText(textToWrite);
    }
}
