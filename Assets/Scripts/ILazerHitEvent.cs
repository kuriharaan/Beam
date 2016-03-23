using UnityEngine;
using System.Collections;

public interface ILazerHitEvent
{
    void OnLazerHit(RaycastHit hitInfo);
}
