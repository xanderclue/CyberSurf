using UnityEngine;
using TMPro;
public class downLetterScript : SelectedObject
{
    [SerializeField]
    TextMeshPro myLetter;
    public override void selectSuccessFunction()
    {
        BuildDebugger.WriteLine("TextMesh is " + myLetter.text);
        char letter = myLetter.text[0];
        BuildDebugger.WriteLine("Letter is " + letter);
        if (letter <= 'A')
        {
            BuildDebugger.WriteLine(letter + " IS <=A");
            letter = 'Z';
            BuildDebugger.WriteLine("Set Letter to " + letter);
        }
        else
        {
            BuildDebugger.WriteLine(letter + " ISNOT <=A");
            --letter;
            BuildDebugger.WriteLine("Set Letter to " + letter);
        }
        myLetter.SetText(letter.ToString());
        BuildDebugger.WriteLine("TextMesh Set to " + myLetter.text);
    }
}