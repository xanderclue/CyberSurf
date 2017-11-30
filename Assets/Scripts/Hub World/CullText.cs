using UnityEngine;
using TMPro;
public class CullText : MonoBehaviour
{
    private void Awake()
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        if (null != text)
            text.enableCulling = true;
        Destroy(this);
    }
}