using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void OnPreRender ()
    {
        var rot = transform.rotation;
        rot.y = Mathf.Sin(Time.time) * Mathf.PI * 0.25f;
        transform.rotation = rot;
    }
}
