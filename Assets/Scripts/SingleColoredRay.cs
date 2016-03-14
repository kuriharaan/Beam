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

        Vector3 castPosition = transform.position;
        Vector3 forward = transform.forward;
        for (int i = 0; i < 2; ++i  )
        {
            RaycastHit hitInfo;
            if (!Physics.Raycast(castPosition, forward, out hitInfo))
            {
                break;
            }

            hitPositions.Add(hitInfo.point);

            castPosition = hitInfo.point;
            forward = Vector3.Reflect(forward, hitInfo.normal);
        }

        Debug.Log(hitPositions.Count);

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
            vertices[1] = transform.InverseTransformPoint(hitPositions[0]);
        }
        else
        {
            vertices[1] = transform.InverseTransformPoint(vertices[1]);
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
