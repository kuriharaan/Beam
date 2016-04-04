using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EzBeamLineRenderer : MonoBehaviour
{
    [SerializeField]
    Material material;

    EzBeam beam;
    LineRenderer lineRenderer;

    void Start ()
    {
        beam = GetComponent<EzBeam>();
        if( null == beam )
        {
            Destroy(this);
            return;
        }

        lineRenderer = GetComponent<LineRenderer>();
        if( null == lineRenderer )
        {
            Destroy(this);
            return;
        }
    }

    void LateUpdate ()
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        Vector3[] vertices = new Vector3[beam.PointList.Count + 1];
        vertices[0] = transform.position;

        for (int i = 0; i < beam.PointList.Count; ++i)
        {
            vertices[i + 1] = beam.PointList[i].position;
        }

        lineRenderer.SetPositions(vertices);
    }
}
