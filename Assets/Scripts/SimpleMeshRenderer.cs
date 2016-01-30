using UnityEngine;
using System.Collections;

public class SimpleMeshRenderer : MonoBehaviour
{
    [SerializeField]
    Material material;

    Mesh       mesh;
    MeshFilter meshFilter;

    Vector3[]  vertices;
    int[]      triangles;
    Vector2[]  uvs;

    // Use this for initialization
    void Start()
    {
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if( null != meshRenderer )
        {
            meshRenderer.material = material;
        }
        meshFilter = gameObject.AddComponent<MeshFilter>();
        if( null == meshFilter )
        {
            return;
        }

        mesh      = new Mesh();
        mesh.name = "SimplePlane";

        Vector3[] vertices = new Vector3[]
        {
            new Vector3( 0.5f,  0.5f, 0.0f ),
            new Vector3(-0.5f, -0.5f, 0.0f ),
            new Vector3(-0.5f,  0.5f, 0.0f ),
            new Vector3( 0.5f, -0.5f, 0.0f )
        };
        int[] triangles = new int[]
        {
            //0, 1, 2,
            //0,
            0, 2, 1, 3, 0,
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

        mesh.vertices = vertices;
        mesh.SetIndices(triangles, MeshTopology.LineStrip, 0);
        mesh.uv        = uv;
        mesh.colors    = colors;
        //mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}
