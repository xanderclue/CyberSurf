using UnityEngine;
using TMPro;
public class downLetterScript : SelectedObject
{
    [SerializeField]
    TextMeshPro myLetter;
    public override void selectSuccessFunction()
    {
        char letter = myLetter.text[0];
        --letter;
        if (letter < 'A')
            letter = 'Z';
        myLetter.SetText(letter.ToString());
    }
}