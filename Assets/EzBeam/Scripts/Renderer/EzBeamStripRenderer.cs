using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EzBeamStripRenderer : MonoBehaviour, IEzBeamRenderer
{
    [SerializeField]
    Material material;

    EzBeam beam;

    Mesh mesh;
    MeshFilter meshFilter;

    public void OnPointUpdated()
    {
        CreateMesh();
        UpdateLineStrip();
    }

    void Start ()
    {
        beam = GetComponent<EzBeam>();
        if( null == beam )
        {
            Destroy(this);
            return;
        }

        CreateMesh();
    }

    void CreateMesh()
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (null == meshRenderer)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;

        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (null == meshFilter)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        if (null == mesh)
        {
            mesh = new Mesh();
            mesh.name = "Ray";
        }
    }

    void UpdateLineStrip()
    {
        Vector3[] vertices = new Vector3[beam.PointList.Count + 1];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < beam.PointList.Count; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(beam.PointList[i].position);
        }

        int[] triangles = new int[beam.PointList.Count + 1];
        for (int i = 0; i < beam.PointList.Count + 1; ++i)
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
