using UnityEngine;
using System.Collections;

public class SingleColoredRay : MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    Color rayColor;

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
        UpdateColor();
    }

    void Update()
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        Vector3[] vertices = new Vector3[]
        {
            transform.position,
            transform.position + transform.forward * 100.0f
        };


        int[] triangles = new int[]
        {
            0, 1
        };

        mesh.vertices = vertices;
        mesh.SetIndices(triangles, MeshTopology.LineStrip, 0);
        mesh.colors = new Color[] { rayColor, rayColor };
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}
