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

    public void setRandomOffset()
    {
        gameObject.GetComponent<Renderer>().material.SetTextureOffset("_EffectsLayer1DistMask",
            new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
    }
}