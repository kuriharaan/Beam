﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EzBeamRenderer : MonoBehaviour
{
    [SerializeField]
    Material material;

    EzBeam beam;

    Mesh mesh;
    MeshFilter meshFilter;

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


    void LateUpdate ()
    {
        CreateMesh();
        UpdateColor();
    }

    void UpdateColor()
    {
        Vector3[] vertices = new Vector3[beam.vertexList.Count + 1];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < beam.vertexList.Count; ++i)
        {
            vertices[i + 1] = transform.InverseTransformPoint(beam.vertexList[i]);
        }

        int[] triangles = new int[beam.vertexList.Count + 1];
        for (int i = 0; i < beam.vertexList.Count + 1; ++i)
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
