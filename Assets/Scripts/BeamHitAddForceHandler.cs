using UnityEngine;
using System.Collections;

public class BeamHitAddForceHandler : MonoBehaviour, IBeamHitEvent
{
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if( null == rigidBody )
        {
            Destroy(this);
        }
    }

    public void OnBeamHit(RaycastHit hitInfo)
    {
        //gameObject.transform.position += hitInfo.normal * -0.1f;
        rigidBody.AddForce(Vector3.one * 1.0f);
    }
}
