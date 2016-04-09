using UnityEngine;
using System.Collections;

public class TestLineRenderer : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
    }

    void LateUpdate()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, gameObject.transform.position + transform.rotation * new Vector3(0.0f, 0.0f, 100.0f));
    }
}
