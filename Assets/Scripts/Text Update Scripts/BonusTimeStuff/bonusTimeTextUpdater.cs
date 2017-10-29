using UnityEngine;
using TMPro;
public class bonusTimeTextUpdater : MonoBehaviour
{
    private static readonly int addTimeHash = Animator.StringToHash("AddTime");
    [SerializeField] private Color myColor;
    private TextMeshProUGUI element = null;
    private Animator myAnimator = null;
    private void Awake()
    {
        element = GetComponent<TextMeshProUGUI>();
        myAnimator = GetComponent<Animator>();
        element.overrideColorTags = true;
        element.color = myColor;
    }
    public void play(string bonusTime)
    {
        element.SetText(bonusTime);
        myAnimator.SetBool(addTimeHash, true);
    }
    public void animationEnded()
    {
        myAnimator.SetBool(addTimeHash, false);
    }
    private void Update()
    {
        element.color = myColor;
    }
}