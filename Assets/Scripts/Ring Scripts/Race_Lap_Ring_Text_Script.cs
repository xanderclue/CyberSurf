using TMPro;
public class Race_Lap_Ring_Text_Script : UnityEngine.MonoBehaviour
{
    private TextMeshPro element = null;
    private void Awake() => element = GetComponent<TextMeshPro>();
    private void Start() => CurrLapChanged();
    private void OnEnable() => RingProperties.laptext.OnCurrLapChanged += CurrLapChanged;
    private void OnDisable() => RingProperties.laptext.OnCurrLapChanged -= CurrLapChanged;
    private void CurrLapChanged()
    {
        if ((GameMode.Race == GameManager.gameMode || GameMode.Cursed == GameManager.gameMode) && RingProperties.laptext.CurrLap != GameManager.MaxLap)
            element.SetText("Checkpoint");
        else
            element.SetText("Exit");
    }
}