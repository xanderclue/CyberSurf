using UnityEngine;
using TMPro;
public class upLetterScript : SelectedObject
{
    [SerializeField]
    private TextMeshPro textMesh;
    public override void SuccessFunction()
    {
        textMesh.ForceMeshUpdate();
        char letter = textMesh.GetParsedText()[0];
        ++letter;
        if (letter > 'Z')
            letter = 'A';
        textMesh.SetText(letter.ToString());
    }
}