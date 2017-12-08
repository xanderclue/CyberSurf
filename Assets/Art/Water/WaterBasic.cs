namespace UnityStandardAssets.Water
{
    using UnityEngine;
    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        private void Update()
        {
            Renderer r = GetComponent<Renderer>();
            if (!r)
                return;
            Material mat = r.sharedMaterial;
            if (!mat)
                return;
            Vector4 offset = mat.GetVector("WaveSpeed") * (mat.GetFloat("_WaveScale") * Time.time * 0.05f);
            mat.SetVector("_WaveOffset", new Vector4(
                Mathf.Repeat(offset.x, 1.0f),
                Mathf.Repeat(offset.y, 1.0f),
                Mathf.Repeat(offset.z, 1.0f),
                Mathf.Repeat(offset.w, 1.0f)));
        }
    }
}