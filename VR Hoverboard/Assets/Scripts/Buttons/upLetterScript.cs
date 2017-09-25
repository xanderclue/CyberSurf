using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class upLetterScript : SelectedObject
{
    [SerializeField]
    TextMeshPro myLetter;
    void Start()
    {

    }

    public override void deSelectedFunction()
    {

    }

    public override void selectedFuntion()
    {

    }

    public override void selectSuccessFunction()
    {
        char letter = myLetter.text[0];
        letter++;
        if (letter >= 91)
        {
            letter = (char)65;
        }
        myLetter.SetText(letter.ToString());
    }
}
