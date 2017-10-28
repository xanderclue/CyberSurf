using UnityEngine;
using TMPro;

public class bonusTimeTextUpdater : MonoBehaviour
{
    private static readonly int addTimeHash = Animator.StringToHash("AddTime");
    [SerializeField] private Color myColor;
    private TextMeshProUGUI element;
    private Animator myAnimator;

    private void Awake()
    {
        element = gameObject.GetComponent<TextMeshProUGUI>();
        myAnimator = gameObject.GetComponent<Animator>();
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