using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(EzBeam))]
public class EzBeamEditor : Editor
{
    static List<MonoBehaviour> allMonobehaviourList = new List<MonoBehaviour>();
    static List<GameObject> allGameObjectList = new List<GameObject>();

    [MenuItem("GameObject/3D Object/EzBeamLineRenderer")]
    static void CreateObjectEzBeamLineRenderer()
    {
        var obj = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath("Assets/EzBeam/Prefabs/EzBeamLineRendererPrefab.prefab")) as GameObject;

        GameObject go = Selection.activeObject as GameObject;
        if( null != go )
        {
            var prefabType = PrefabUtility.GetPrefabType(go);
            if(
                ( PrefabType.None == prefabType) ||
                ( PrefabType.DisconnectedModelPrefabInstance == prefabType) ||
                ( PrefabType.DisconnectedPrefabInstance == prefabType) ||
                ( PrefabType.MissingPrefabInstance == prefabType) ||
                ( PrefabType.ModelPrefabInstance == prefabType) ||
                ( PrefabType.PrefabInstance == prefabType)
            )
            {
                obj.transform.SetParent(go.transform, false);
            }
        }

        Selection.activeObject = obj;
    }


    SerializedProperty popObjectsSize;

    void OnEnable()
    {
        popObjectsSize = serializedObject.FindProperty("popObjects.Array.size");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUI.indentLevel = 0;
        EditorGUILayout.LabelField(new GUIContent("Pop object on hit."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultPopObjectPrefab"), new GUIContent("Default"));

        EditorGUILayout.LabelField(new GUIContent("Optional objects with tag."));

        EditorGUI.indentLevel = 1;
        EditorGUILayout.PropertyField(popObjectsSize);

        EditorGUI.indentLevel = 2;

        for (int i = 0; i < popObjectsSize.intValue; ++i)
        {
            SerializedProperty tag = serializedObject.FindProperty(string.Format("popObjects.Array.data[{0}].tag", i));
            SerializedProperty prefab = serializedObject.FindProperty(string.Format("popObjects.Array.data[{0}].gameObject", i));

            EditorGUILayout.BeginHorizontal();

            tag.stringValue = EditorGUILayout.TagField(tag.stringValue);
            EditorGUILayout.PropertyField(prefab, GUIContent.none);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel = 0;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enableReflectionMax"), new GUIContent("Reflection limitation"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reflectionMax"), GUIContent.none);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("lengthMax"), new GUIContent("length"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("force"), new GUIContent("force"));

        EzBeam beam = serializedObject.targetObject as EzBeam;
        IEzBeamRenderer renderer = beam.gameObject.GetComponent<IEzBeamRenderer>();
        if( null == renderer )
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(30);
            if( GUILayout.Button("Add Renderer") )
            {
                //string[] guids = AssetDatabase.FindAssets("t:Script");
                //
                //foreach (string guid in guids)
                //{
                //    Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
                //}

                EzBeam[] beams = FindObjectsOfType(typeof(EzBeam)) as EzBeam[];

                foreach (var b in beams)
                {
                    Debug.Log(b.name);
                }

            }
            GUILayout.Space(30);


            EditorGUILayout.EndVertical();

        }

        serializedObject.ApplyModifiedProperties();
    }

    public static List<GameObject> AllObjects
    {
        get
        {
            if (frameCount != gameobjectLastUpdateFrame)
            {
                UpdateAllObjectList();
                gameobjectLastUpdateFrame = frameCount;
            }

            return new List<GameObject>(allGameObjectList);
        }
    }

    public static long frameCount
    {
        get;
        private set;
    }

    static long monobehaviourLastUpdateFrame = 0, gameobjectLastUpdateFrame = 0;


    static void UpdateAllMonobehaviourList()
    {
        allMonobehaviourList.Clear();
        allMonobehaviourList.AddRange(GetComponentsInList<MonoBehaviour>(AllObjects));
    }

    public static IEnumerable<T> GetComponentsInList<T>(IEnumerable<GameObject> gameObjects) where T : Component
    {
        var componentList = new List<T>();

        foreach (var obj in gameObjects)
        {
            var component = obj.GetComponent<T>();
            if (component != null)
            {
                componentList.Add(component);
            }
        }
        return componentList;
    }


    static void UpdateAllObjectList()
    {
        allGameObjectList.Clear();
        foreach (GameObject obj in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {

            if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
                continue;

            if (Application.isEditor)
            {
                string sAssetPath = AssetDatabase.GetAssetPath(obj.transform.root.gameObject);
                if (!string.IsNullOrEmpty(sAssetPath))
                    continue;
            }

            allGameObjectList.Add(obj);
        }
    }

}

