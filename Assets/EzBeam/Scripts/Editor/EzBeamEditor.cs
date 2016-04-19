﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    int popupIndex = 0;
    List<System.Type> typeList = new List<System.Type>();

    void OnEnable()
    {
        popObjectsSize = serializedObject.FindProperty("popObjects.Array.size");

        typeList.Clear();
        var interfaceType = typeof(IEzBeamRenderer);
        var assembly = System.Reflection.Assembly.GetAssembly(interfaceType);
        foreach (System.Type type in assembly.GetTypes())
        {
            if (type.GetInterfaces().Contains(interfaceType))
            {
                typeList.Add(type);
            }
        }
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

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Select EzBeam Renderer");
            string[] options = new string[typeList.Count];
            for (int i = 0; i < typeList.Count; ++i )
            {
                options[i] = typeList[i].Name;
            }
            popupIndex = EditorGUILayout.Popup(popupIndex, options);

            GUILayout.Space(10);
            if( GUILayout.Button("Add Renderer") )
            {
                beam.gameObject.AddComponent(typeList[popupIndex]);
            }



            EditorGUILayout.EndVertical();

        }

        serializedObject.ApplyModifiedProperties();
    }

}

