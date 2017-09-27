using UnityEngine;
using TMPro;
public class upLetterScript : SelectedObject
{
    [SerializeField]
    TextMeshPro myLetter;
    public override void selectSuccessFunction()
    {
        char letter = myLetter.text[0];
        ++letter;
        if (letter > 'Z')
            letter = 'A';
        myLetter.SetText(letter.ToString());
    }
}