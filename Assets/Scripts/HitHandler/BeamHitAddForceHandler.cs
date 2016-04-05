using UnityEngine;
using System.Collections;

public class BeamHitAddForceHandler : MonoBehaviour, IBeamHitEvent
{
    public float force = 1.0f;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if( null == rigidBody )
        {
            Destroy(this);
        }
    }

    public void OnBeamHit(BeamHitInfo hitInfo)
    {
        rigidBody.AddForceAtPosition(hitInfo.incidence * force, hitInfo.position);
    }
}
