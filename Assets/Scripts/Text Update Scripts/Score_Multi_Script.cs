using UnityEngine;
using TMPro;
public class Score_Multi_Script : MonoBehaviour
{
    private ScoreManager manager = null;
    private TextMeshProUGUI element = null;
    private void Start()
    {
        manager = GameManager.instance.scoreScript;
        element = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        element.SetText(" " + manager.score_multiplier + "X Score");
    }
}