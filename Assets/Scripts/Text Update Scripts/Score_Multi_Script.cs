using TMPro;
public class Score_Multi_Script : UnityEngine.MonoBehaviour
{
    private TextMeshProUGUI element = null;
    private void Start() => element = GetComponent<TextMeshProUGUI>();
    private void Update() => element.SetText(" " + ScoreManager.score_multiplier + "X Score");
}