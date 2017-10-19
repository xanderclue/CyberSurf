using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuAnimationFunctions : MonoBehaviour
{
    [SerializeField] Texture2D fullMaskTex;
    [SerializeField] Texture2D partialMaskTex;
    bool FullTex = true;

    public void switchMaskTextures()
    {
        if (FullTex)
        {
            gameObject.GetComponent<Renderer>().material.SetTexture("_EffectsLayer1DistMask", fullMaskTex);
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetTexture("_EffectsLayer1DistMask", partialMaskTex);
        }
        FullTex = !FullTex;
    }
}
