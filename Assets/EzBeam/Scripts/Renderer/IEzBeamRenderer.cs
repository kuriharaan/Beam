using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IEzBeamRenderer : IEventSystemHandler
{
    void OnUpdate();
}
