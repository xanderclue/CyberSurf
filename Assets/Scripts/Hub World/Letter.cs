using UnityEngine;
using TMPro;
public class Letter : MonoBehaviour
{
    [SerializeField] TextMeshPro letterText = null;
    [SerializeField] EventSelectedObject letterUpButton = null, letterDownButton = null;
    private char letter = 'A';
    public char CharValue { get { return letter; } set { letter = value; OnLetterChanged?.Invoke(); } }
    public delegate void LetterChangedEvent();
    public event LetterChangedEvent OnLetterChanged;
    private void UpdateText() => letterText.SetText(letter.ToString());
    private void OnEnable()
    {
        letterUpButton.OnSelectSuccess += LetterUp;
        letterDownButton.OnSelectSuccess += LetterDown;
        OnLetterChanged += UpdateText;
        UpdateText();
    }
    private void OnDisable()
    {
        letterUpButton.OnSelectSuccess -= LetterUp;
        letterDownButton.OnSelectSuccess -= LetterDown;
        OnLetterChanged -= UpdateText;
    }
    private void LetterUp()
    {
        if ((++letter) > 'Z')
            letter = 'A';
        OnLetterChanged?.Invoke();
    }
    private void LetterDown()
    {
        if ((--letter) < 'A')
            letter = 'Z';
        OnLetterChanged?.Invoke();
    }
}