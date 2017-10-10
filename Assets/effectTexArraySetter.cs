using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectTexArraySetter : MonoBehaviour 
{
    [SerializeField] Texture[] effectTextures;

    Texture2DArray newArray;
    // Use this for initialization
    void OnEnable ()
    {
        newArray = new Texture2DArray(effectTextures[0].width, effectTextures[0].height,
            effectTextures.Length, TextureFormat.RGBA32, false);

        Graphics.CopyTexture(effectTextures[0], newArray);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
