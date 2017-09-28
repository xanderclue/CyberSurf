using UnityEngine;
using TMPro;
public class upLetterScript : SelectedObject
{
    [SerializeField]
    private TextMeshPro textMesh;
    private char letter = 'A';
    public override void selectSuccessFunction()
    {
        ++letter;
        if (letter > 'Z')
            letter = 'A';
        textMesh.SetText(letter.ToString());
    }
}