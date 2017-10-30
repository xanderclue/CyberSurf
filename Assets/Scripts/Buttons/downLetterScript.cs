using UnityEngine;
using TMPro;
public class downLetterScript : SelectedObject
{
    [SerializeField] private TextMeshPro textMesh = null;
    protected override void SuccessFunction()
    {
        char letter = textMesh.GetParsedText()[0];
        --letter;
        if (letter < 'A')
            letter = 'Z';
        textMesh.SetText(letter.ToString());
    }
}