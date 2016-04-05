using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IBeamHitEvent : IEventSystemHandler
{
    void OnBeamHit(RaycastHit hitInfo);
}
