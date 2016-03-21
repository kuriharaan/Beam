﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SingleColoredRay : MonoBehaviour
{
    [System.Serializable]
    public struct PopObject
    {
        public string tag;
        public GameObject gameObject;
    }

    [SerializeField]
    Material material;

    [SerializeField]
    PopObject[] popObjects;

    Mesh       mesh;
    MeshFilter meshFilter;

    List<Vector3> vertexList = new List<Vector3>();
    List<Vector3> normalList = new List<Vector3>();

    List<GameObject> hitInfomations = new List<GameObject>();
    List<GameObject> popedObjects = new List<GameObject>();

    GameObject defaultPopObject = null;

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

        SetupDefaultPopObject();
    }

    void Update()
    {
        vertexList.Clear();
        normalList.Clear();

        Vector3 castPosition = transform.position;
        Vector3 forward = transform.forward;
        int diffTopIndex = int.MaxValue;

        for (int i = 0; i < 100; ++i  )
        {
            RaycastHit hitInfo;
            if (!Physics.Raycast(castPosition, forward, out hitInfo))
            {
                break;
            }

            if( hitInfomations.Count <= i )
            {
                diffTopIndex = Mathf.Min(diffTopIndex, i);
                hitInfomations.Add(hitInfo.collider.gameObject);
            }
            else if( hitInfomations[i] != hitInfo.collider.gameObject )
            {
                diffTopIndex = Mathf.Min(diffTopIndex, i);
                hitInfomations[i] = hitInfo.collider.gameObject;
            }

            vertexList.Add(hitInfo.point);
            normalList.Add(hitInfo.normal);

            castPosition = hitInfo.point;
            forward = Vector3.Reflect(forward, hitInfo.normal);
        }

        hitInfomations.RemoveRange(vertexList.Count, hitInfomations.Count - vertexList.Count);

        vertexList.Add(forward * 100.0f + castPosition);

        UpdateColor();
        UpdatePopObjects(diffTopIndex);
    }

    void SetupDefaultPopObject()
    {
        foreach (var p in popObjects)
        {
            if (string.IsNullOrEmpty(p.tag) )
            {
                defaultPopObject = p.gameObject;
                break;
            }
        }
    }

    GameObject FindPopObject(string tag)
    {
        foreach (var p in popObjects)
        {
            if (p.tag == tag)
            {
                return p.gameObject;
            }
        }

        return defaultPopObject;
    }

    void UpdatePopObjects(int diffTopIndex)
    {
        for (int i = 0; i < hitInfomations.Count; ++i)
        {
            if (diffTopIndex <= i)
            {
                GameObject popObject = FindPopObject(hitInfomations[i].gameObject.tag);

                if ((popedObjects.Count > i) && (null != popedObjects))
                {
                    Destroy(popedObjects[i]);
                    popedObjects[i] = Instantiate(popObject, vertexList[i], Quaternion.LookRotation(normalList[i])) as GameObject;
                }
                else
                {
                    popedObjects.Add(Instantiate(popObject, vertexList[i], Quaternion.LookRotation(normalList[i])) as GameObject);
                }
            }
            else
            {
                popedObjects[i].transform.position = vertexList[i];
                popedObjects[i].transform.rotation = Quaternion.LookRotation(normalList[i]);
            }
        }

        for (int i = popedObjects.Count - 1; i >= 0; --i)
        {
            if (hitInfomations.Count > i)
            {
                break;
            }

            Destroy(popedObjects[i]);
            popedObjects.RemoveAt(i);
        }
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
