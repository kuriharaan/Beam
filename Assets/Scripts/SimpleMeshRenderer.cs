using UnityEngine;
using System.Collections;

public class SimpleMeshRenderer : MonoBehaviour
{
    Mesh       mesh;
    MeshFilter meshFilter;

    Vector3[]  vertices;
    int[]      triangles;
    Vector2[]  uvs;

    // Use this for initialization
    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        if( null == meshFilter )
        {
            return;
        }

        mesh      = new Mesh();
        mesh.name = "SimplePlane";

        Debug.Log(mesh.GetTopology(0));

        Vector3[] vertices = new Vector3[]
        {
            new Vector3( 0.5f,  0.5f, 0.0f ),
            new Vector3(-0.5f, -0.5f, 0.0f ),
            new Vector3(-0.5f,  0.5f, 0.0f ),
            new Vector3( 0.5f, -0.5f, 0.0f )
        };
        int[] triangles = new int[]
        {
            0, 1, 2,
            3, 1, 0
        };
        Vector2[] uv = new Vector2[]
        {
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        };

        Color[] colors = new Color[vertices.Length];
        colors[0] = Color.red;
        colors[1] = Color.blue;
        colors[2] = Color.yellow;
        colors[3] = Color.green;

        mesh.vertices  = vertices;
        mesh.triangles = triangles;
        mesh.uv        = uv;
        mesh.colors    = colors;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mesh.SetIndices(triangles, MeshTopology.Lines, 0);

        meshFilter.mesh = mesh;
    }
}
