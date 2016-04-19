using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EzBeamLineRenderer : MonoBehaviour, IEzBeamRenderer
{
    EzBeam beam;
    LineRenderer lineRenderer;

    public void OnUpdate()
    {
        UpdateColor();
    }

    void Start ()
    {
        beam = GetComponent<EzBeam>();
        if( null == beam )
        {
            if (Application.isPlaying)
            {
                Destroy(this);
            }
            else
            {
                DestroyImmediate(this);
            }
            return;
        }

        lineRenderer = GetComponent<LineRenderer>();
    }

    void OnPreRender()
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        if( null == lineRenderer )
        {
            return;
        }

        lineRenderer.SetVertexCount(beam.PointList.Count + 1);
        lineRenderer.SetPosition(0, transform.position);
        for( int i = 0; i < beam.PointList.Count; ++i )
        {
            lineRenderer.SetPosition(i + 1, beam.PointList[i].position);
        }
    }
}
