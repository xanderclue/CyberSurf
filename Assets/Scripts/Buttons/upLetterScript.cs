using UnityEngine;
using TMPro;
public class upLetterScript : SelectedObject
{
    [SerializeField] private TextMeshPro textMesh;
    protected override void SuccessFunction()
    {
        char letter = textMesh.GetParsedText()[0];
        ++letter;
        if (letter > 'Z')
            letter = 'A';
        textMesh.SetText(letter.ToString());
    }
}