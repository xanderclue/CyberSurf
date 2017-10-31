using UnityEngine;
using Xander.NullConversion;
public class invertCollision : MonoBehaviour
{
    private void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().ConvertNull()?.mesh;
        if (null != mesh)
        {
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; ++i)
            {
                normals[i] = -normals[i];
            }
            int[] triangles;
            int temp;
            for (int m = 0; m < mesh.subMeshCount; ++m)
            {
                triangles = mesh.GetTriangles(m);
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    temp = triangles[j];
                    triangles[j] = triangles[j + 1];
                    triangles[j + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        }
        Destroy(this);
    }
}