using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleColoredRay : MonoBehaviour
{
    [SerializeField]
    Material material;

    Mesh       mesh;
    MeshFilter meshFilter;

    List<Vector3> vertexList = new List<Vector3>();

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
    }

    void Update()
    {
        vertexList.Clear();

        Vector3 castPosition = transform.position;
        Vector3 forward = transform.forward;
        for (int i = 0; i < 100; ++i  )
        {
            RaycastHit hitInfo;
            if (!Physics.Raycast(castPosition, forward, out hitInfo))
            {
                break;
            }

            vertexList.Add(hitInfo.point);

            castPosition = hitInfo.point;
            forward = Vector3.Reflect(forward, hitInfo.normal);
        }

        vertexList.Add(forward * 100.0f + castPosition);

        UpdateColor();
    }

    void UpdateColor()
    {
        Vector3[] vertices = new Vector3[vertexList.Count + 1];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexList.Count; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(vertexList[i]);
        }

        int[] triangles = new int[vertexList.Count + 1];
        for (int i = 0; i < vertexList.Count + 1; ++i)
        {
            triangles[i] = i;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(triangles, MeshTopology.LineStrip, 0);
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}
