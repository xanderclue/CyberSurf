using UnityEngine;
public class menuAnimationFunctions : MonoBehaviour
{
    [SerializeField] private Texture2D fullMaskTex = null, partialMaskTex = null;
    private bool fullTex = true;
    public void switchMaskTextures()
    {
        GetComponent<Renderer>().material.SetTexture("_EffectsLayer1DistMask", fullTex ? fullMaskTex : partialMaskTex);
        fullTex = !fullTex;
    }
    public void setRandomOffset()
    {
        GetComponent<Renderer>().material.SetTextureOffset("_EffectsLayer1DistMask", new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
    }
}