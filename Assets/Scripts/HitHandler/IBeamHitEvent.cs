using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public struct BeamHitInfo
{
    public Vector3 normal;
    public Vector3 position;
    public Vector3 incidence;
}

public interface IBeamHitEvent : IEventSystemHandler
{
    void OnBeamHit(BeamHitInfo hitInfo);
}
