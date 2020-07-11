
using UnityEngine;

public class WorldEditor : MonoBehaviour
{
    public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
    }

}
