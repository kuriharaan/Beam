using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SingleColoredRay : MonoBehaviour
{
    [System.Serializable]
    public struct PopObject
    {
        public string tag;
        public GameObject popObject;
    }

    [SerializeField]
    Material material;

    [SerializeField]
    GameObject hitParticle;

    [SerializeField]
    HitObject[] popObjects;

    Mesh       mesh;
    MeshFilter meshFilter;

    List<Vector3> vertexList = new List<Vector3>();

    struct HitObject
    {
        public int hitIndex;
        public GameObject hitObject;
        public GameObject popObject;
    };

    List<HitObject> hitObjects = new List<HitObject>();

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

            int index = hitObjects.FindIndex(x => x.hitObject == hitInfo.collider.gameObject);
            if( 0 <= index )
            {
                hitObjects[index].popObject.transform.position = hitInfo.point;
                hitObjects[index].popObject.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            }
            else
            {
                HitObject hitObject = new HitObject();
                hitObject.popObject = Instantiate(hitParticle, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                hitObject.hitObject = hitInfo.collider.gameObject;

                hitObjects.Add(hitObject);
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
