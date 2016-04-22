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
    public GameObject defaultPopObjectPrefab;

    [SerializeField]
    PopObject[] popObjects = new PopObject[] {};

    public float force;

    public struct Point
    {
        public Vector3 position;
        public Vector3 normal;
    };
    List<Point> pointList = new List<Point>();

    struct PopedObject
    {
        public GameObject defaultObject;
        public GameObject optionalObject;

        public void SetTransform(Vector3 position, Quaternion rotation)
        {
            if( null != defaultObject )
            {
                defaultObject.transform.position = position;
                defaultObject.transform.rotation = rotation;
            }

            if( null != optionalObject )
            {
                optionalObject.transform.position = position;
                optionalObject.transform.rotation = rotation;
            }
        }
    };

    List<GameObject> hitInfomations = new List<GameObject>();
    List<PopedObject> popedObjects = new List<PopedObject>();

    void LateUpdate()
    {
        UpdateHitInformation();
    }

    void Start()
    {
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
            (recieveTarget, y) => recieveTarget.OnPointUpdated()
        );
    }

    void UpdatePopObjects(int diffTopIndex)
    {
        for (int i = 0; i < hitInfomations.Count; ++i)
        {
            var point = pointList[i];
            if (diffTopIndex <= i)
            {
                if ((popedObjects.Count > i) && (null != popedObjects))
                {
                    DestroyPopedObject(popedObjects[i]);
                    popedObjects[i] = InstantiatePopedObject(hitInfomations[i].gameObject.tag, point);
                }
                else
                {
                    popedObjects.Add(InstantiatePopedObject(hitInfomations[i].gameObject.tag, point));
                }
            }
            else
            {
                popedObjects[i].SetTransform(point.position, Quaternion.LookRotation(point.normal));
            }
        }

        for (int i = popedObjects.Count - 1; i >= 0; --i)
        {
            if (hitInfomations.Count > i)
            {
                break;
            }

            DestroyPopedObject(popedObjects[i]);
            popedObjects.RemoveAt(i);
        }
    }

    void DestroyPopedObject(PopedObject obj)
    {
        if( null != obj.defaultObject )
        {
            DestroyPopedObject(obj.defaultObject);
        }

        if( null != obj.optionalObject )
        {
            DestroyPopedObject(obj.optionalObject);
        }
    }

    void DestroyPopedObject(GameObject obj)
    {
        var particle = obj.GetComponent<ParticleSystem>();
        if( null == particle )
        {
            particle = obj.GetComponentInChildren<ParticleSystem>();
        }

        if( null != particle )
        {
            particle.Stop();
            Destroy(obj, particle.duration);
        }
        else
        {
            Destroy(obj);
        }
    }

    PopedObject InstantiatePopedObject(string tag, Point point)
    {
        PopedObject popedObject = new PopedObject();
        if (null != defaultPopObjectPrefab)
        {
            popedObject.defaultObject = Instantiate(defaultPopObjectPrefab, point.position, Quaternion.LookRotation(point.normal)) as GameObject;
        }

        var optional = FindOptionalPopObject(tag);
        if( null != optional )
        {
            popedObject.optionalObject = Instantiate(optional, point.position, Quaternion.LookRotation(point.normal)) as GameObject;
        }

        return popedObject;
    }

    GameObject FindOptionalPopObject(string tag)
    {
        foreach (var p in popObjects)
        {
            if (p.tag == tag)
            {
                return p.gameObject;
            }
        }

        return null;
    }
}
