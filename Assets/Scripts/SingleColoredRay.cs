using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleColoredRay : MonoBehaviour
{
    [SerializeField]
    Material material;

    Mesh       mesh;
    MeshFilter meshFilter;

    Vector3[]  vertices;
    int[]      triangles;
    Vector2[]  uvs;

    List<Vector3> hitPositions = new List<Vector3>();

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
        mesh.name = "Ray";
        UpdateColor();
    }

    void Update()
    {
        hitPositions.Clear();
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
        {
            hitPositions.Add(hitInfo.point);
        }

        UpdateColor();

    }

    void UpdateColor()
    {
        Vector3[] vertices = new Vector3[]
        {
            Vector3.zero,
            transform.forward * 100.0f
        };

        if( 0 < hitPositions.Count )
        {
            vertices[1] = hitPositions[0] - transform.position;
        }

        int[] triangles = new int[]
        {
            0, 1
        };

        mesh.vertices = vertices;
        mesh.SetIndices(triangles, MeshTopology.LineStrip, 0);
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}
