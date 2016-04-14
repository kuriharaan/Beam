using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EzBeam))]
public class EzBeamEditor : Editor
{

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

        serializedObject.ApplyModifiedProperties();
    }
}

