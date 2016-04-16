using UnityEngine;
using System.Collections;

public class ReflectionPlayer : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {

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
            //transform.position += transform.forward * vertical;
            //GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * vertical);
        }
    }
}
