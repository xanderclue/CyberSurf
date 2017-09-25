using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class downLetterScript : SelectedObject
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
        letter--;
        if (letter <= 64)
        {
            letter = (char)90;
        }
        myLetter.SetText(letter.ToString());
    }
}