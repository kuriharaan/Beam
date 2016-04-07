using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EzBeamLineRenderer : MonoBehaviour
{
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
        lineRenderer.SetVertexCount(beam.PointList.Count + 1);
        lineRenderer.SetPosition(0, transform.position);
        for( int i = 0; i < beam.PointList.Count; ++i )
        {
            lineRenderer.SetPosition(i + 1, beam.PointList[i].position);
        }
    }
}
