using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class EzBeam : MonoBehaviour
{
    [System.Serializable]
    public struct PopObject
    {
        public string tag;
        public GameObject gameObject;
    }

    public List<Point> PointList
    {
        get
        {
            return pointList;
        }
    }

    [SerializeField]
    public int reflectionMax = 0;

    [SerializeField]
    public bool enableReflectionMax = false;

    [SerializeField]
    public float lengthMax = 10000.0f;

    [SerializeField]
    PopObject[] popObjects = new PopObject[] {};

    public float force;

    public struct Point
    {
        public Vector3 position;
        public Vector3 normal;
    };
    List<Point> pointList = new List<Point>();

    List<GameObject> hitInfomations = new List<GameObject>();
    List<GameObject> popedObjects = new List<GameObject>();

    GameObject defaultPopObject = null;

    void LateUpdate()
    {
        UpdateHitInformation();
    }

    void Start()
    {
        SetupDefaultPopObject();
    }

    void UpdateHitInformation()
    {
        pointList.Clear();

        Vector3 castPosition = transform.position;
        Vector3 forward = transform.forward;
        int diffTopIndex = int.MaxValue;

        float distance = Mathf.Max(0.0f, lengthMax);

        bool stopOnReflect = false;
        for (int i = 0; ; ++i  )
        {
            if (enableReflectionMax && (0 <= reflectionMax) && (reflectionMax < i))
            {
                stopOnReflect = true;
                break;
            }

            RaycastHit hitInfo;
            if (!Physics.Raycast(castPosition, forward, out hitInfo, distance))
            {
                break;
            }

            distance = Mathf.Max(0.0f, distance - (castPosition - hitInfo.point).magnitude);

            if( hitInfomations.Count <= i )
            {
                diffTopIndex = Mathf.Min(diffTopIndex, i);
                hitInfomations.Add(hitInfo.collider.gameObject);
            }
            else if( hitInfomations[i] != hitInfo.collider.gameObject )
            {
                diffTopIndex = Mathf.Min(diffTopIndex, i);
                hitInfomations[i] = hitInfo.collider.gameObject;
            }

            Point point;
            point.position = hitInfo.point;
            point.normal   = hitInfo.normal;
            pointList.Add(point);

            if (Application.isPlaying)
            {
                BeamHitInfo info;
                info.position  = hitInfo.point;
                info.normal    = hitInfo.normal;
                info.incidence = forward;
                info.beam      = this;

                ExecuteEvents.Execute<IBeamHitEvent>(
                    hitInfo.collider.gameObject,
                    null,
                    (recieveTarget, y) => recieveTarget.OnBeamHit(info)
                );
            }

            castPosition = hitInfo.point;
            forward = Vector3.Reflect(forward, hitInfo.normal);
        }

        hitInfomations.RemoveRange(pointList.Count, hitInfomations.Count - pointList.Count);

        if (!stopOnReflect )
        {
            Point point;
            point.position = forward * distance + castPosition;
            point.normal   = -point.position;
            pointList.Add(point);
        }

        if( Application.isPlaying )
        {
            UpdatePopObjects(diffTopIndex);
        }


        ExecuteEvents.Execute<IEzBeamRenderer>(
            gameObject,
            null,
            (recieveTarget, y) => recieveTarget.OnUpdate()
        );
    }

    void SetupDefaultPopObject()
    {
        foreach (var p in popObjects)
        {
            if (string.IsNullOrEmpty(p.tag) )
            {
                defaultPopObject = p.gameObject;
                break;
            }
        }
    }

    GameObject FindPopObject(string tag)
    {
        foreach (var p in popObjects)
        {
            if (p.tag == tag)
            {
                return p.gameObject;
            }
        }

        return defaultPopObject;
    }

    void UpdatePopObjects(int diffTopIndex)
    {
        for (int i = 0; i < hitInfomations.Count; ++i)
        {
            GameObject popObject = FindPopObject(hitInfomations[i].gameObject.tag);
            if( null == popObject )
            {
                continue;
            }

            var point = pointList[i];
            if (diffTopIndex <= i)
            {
                if ((popedObjects.Count > i) && (null != popedObjects))
                {
                    Destroy(popedObjects[i]);
                    popedObjects[i] = Instantiate(popObject, point.position, Quaternion.LookRotation(point.normal)) as GameObject;
                }
                else
                {
                    popedObjects.Add(Instantiate(popObject, point.position, Quaternion.LookRotation(point.normal)) as GameObject);
                }
            }
            else
            {
                popedObjects[i].transform.position = point.position;
                popedObjects[i].transform.rotation = Quaternion.LookRotation(point.normal);
            }
        }

        for (int i = popedObjects.Count - 1; i >= 0; --i)
        {
            if (hitInfomations.Count > i)
            {
                break;
            }

            Destroy(popedObjects[i]);
            popedObjects.RemoveAt(i);
        }
    }
}
