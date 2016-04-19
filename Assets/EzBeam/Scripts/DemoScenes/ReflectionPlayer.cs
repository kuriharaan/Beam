using UnityEngine;
using System.Collections;

public class ReflectionPlayer : MonoBehaviour
{
    EzBeam beam;

    // Use this for initialization
    void Start ()
    {
        beam = GetComponentInChildren<EzBeam>();
    }

    // Update is called once per frame
    void Update ()
    {

        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            transform.Rotate(new Vector3(0.0f, horizontal, 0.0f));

        }

        float vertical = Input.GetAxis("Vertical") * 0.5f;
        if (Mathf.Abs(vertical) > 0.1f)
        {
            beam.lengthMax = Mathf.Clamp(beam.lengthMax + vertical, 1.0f, 20.0f);
        }
    }
}
