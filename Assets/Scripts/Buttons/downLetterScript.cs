using UnityEngine;
using TMPro;
public class downLetterScript : SelectedObject
{
    [SerializeField]
    private TextMeshPro textMesh;
    private char letter = 'A';
    public override void selectSuccessFunction()
    {
        --letter;
        if (letter < 'A')
            letter = 'Z';
        textMesh.SetText(letter.ToString());
    }
}