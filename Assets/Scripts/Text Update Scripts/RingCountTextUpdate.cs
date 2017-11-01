using UnityEngine;
using TMPro;
public class RingCountTextUpdate : MonoBehaviour
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
        element.SetText(" " + manager.ringHitCount + " ");
    }
}