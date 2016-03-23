using UnityEngine;
using System.Collections;

public class LazerHitHandler : MonoBehaviour, ILazerHitEvent
{
    public void OnLazerHit(RaycastHit hitInfo)
    {
        gameObject.transform.position += hitInfo.normal * -0.1f;
    }
}
