using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface ILazerHitEvent : IEventSystemHandler
{
    void OnLazerHit(RaycastHit hitInfo);
}
