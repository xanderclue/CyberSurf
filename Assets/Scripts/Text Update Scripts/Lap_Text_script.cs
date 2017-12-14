using UnityEngine;
using TMPro;
public class Lap_Text_script : MonoBehaviour
{
    public delegate void CurrLapChangedEvent();
    public event CurrLapChangedEvent OnCurrLapChanged;
    private TextMeshProUGUI element = null;
    private int currLap;
    public int CurrLap { get { return currLap; } set { currLap = value; OnCurrLapChanged?.Invoke(); } }
    private void Awake() => element = GetComponent<TextMeshProUGUI>();
    private void Start() => CurrLap = 1;
    private void OnEnable() => OnCurrLapChanged += CurrLapChanged;
    private void OnDisable() => OnCurrLapChanged -= CurrLapChanged;
    private void CurrLapChanged() => element.SetText($"Lap\n{currLap} / {GameManager.MaxLap}");
    private void Update()
    {
        if (-1 == GameManager.lastPortalBuildIndex)
            CurrLap = 1;
    }
}