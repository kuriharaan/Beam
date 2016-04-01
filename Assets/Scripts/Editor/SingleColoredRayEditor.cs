using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SingleColoredRay))]
public class SingleColoredRayEditor : Editor
{
    SerializedProperty popObjectsSize;

    void OnEnable()
    {
        popObjectsSize = serializedObject.FindProperty("popObjects.Array.size");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.indentLevel = 0;
        EditorGUILayout.LabelField(new GUIContent("Pop objects on hit"));

        EditorGUI.indentLevel = 1;
        EditorGUILayout.PropertyField(popObjectsSize);

        EditorGUI.indentLevel = 2;

        for (int i = 0; i < popObjectsSize.intValue; ++i)
        {
            SerializedProperty tag        = serializedObject.FindProperty(string.Format("popObjects.Array.data[{0}].tag", i));
            SerializedProperty prefab     = serializedObject.FindProperty(string.Format("popObjects.Array.data[{0}].gameObject", i));

            EditorGUILayout.BeginHorizontal();

            tag.stringValue = EditorGUILayout.TagField(tag.stringValue);
            EditorGUILayout.PropertyField(prefab, GUIContent.none);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel = 0;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enableReflectionMax"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reflectionMax"));

        serializedObject.ApplyModifiedProperties();
    }
}

