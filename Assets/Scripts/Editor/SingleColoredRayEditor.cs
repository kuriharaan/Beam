using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SingleColoredRay))]
public class SingleColoredRayEditor : Editor
{
    enum EType
    {
        AAA,
        BBB,
    }

    string tag = "";
    public override void OnInspectorGUI()
    {
        tag = EditorGUILayout.TagField(tag);
        DrawDefaultInspector();
    }
}
