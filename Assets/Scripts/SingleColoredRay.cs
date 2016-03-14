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

    struct Segment
    {
        public Vector3 p0;
        public Vector3 p1;
    }
    List<Segment> segments = new List<Segment>();

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
        segments.Clear();

        Vector3 castPosition = transform.position;
        Vector3 forward = transform.forward;
        for (int i = 0; i < 100; ++i  )
        {
            RaycastHit hitInfo;
            if (!Physics.Raycast(castPosition, forward, out hitInfo))
            {
                break;
            }

            Segment seg;
            seg.p0 = castPosition;
            seg.p1 = hitInfo.point;
            segments.Add(seg);

            castPosition = hitInfo.point;
            forward = Vector3.Reflect(forward, hitInfo.normal);
        }

        Segment segLast;
        segLast.p0 = castPosition;
        segLast.p1 = forward * 100.0f + castPosition;
        segments.Add(segLast);

        UpdateColor();
    }

    void UpdateColor()
    {
        Vector3[] vertices = new Vector3[segments.Count + 1];
        vertices[0] = segments[0].p0;

        for (int i = 0; i < segments.Count; ++i )
        {
            vertices[i + 1] = transform.InverseTransformPoint(segments[i].p1);
        }

        int[] triangles = new int[segments.Count + 1];
        for( int i = 0; i < segments.Count + 1; ++i )
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
