using UnityEngine;
using System.Collections;

public class BeamHitDestroyHandler : MonoBehaviour, IBeamHitEvent
{
    public void OnBeamHit(BeamHitInfo hitInfo)
    {
        Destroy(gameObject);
    }
}
