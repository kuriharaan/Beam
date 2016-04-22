using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(EzBeam))]
public class EzBeamEditor : Editor
{
    public class BackgroundColorScope : GUI.Scope
    {
        private Color color;
        public BackgroundColorScope(Color color)
        {
            this.color = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }
        protected override void CloseScope()
        {
            GUI.backgroundColor = color;
        }
    }

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

    static Color addRendererButtonColor = new Color(1.0f, 0.7f, 0.7f);

    int popupIndex = 0;
    List<System.Type> typeList = new List<System.Type>();
    ReorderableList list;

    void OnEnable()
    {
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

        list = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("popObjects"),
            true,
            true,
            true,
            true
        );
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            element.FindPropertyRelative("tag").stringValue = EditorGUI.TagField(
                        new Rect(rect.x, rect.y, 120, EditorGUIUtility.singleLineHeight),
                        GUIContent.none,
                        element.FindPropertyRelative("tag").stringValue);

            EditorGUI.PropertyField(
                new Rect(rect.x + 120, rect.y, rect.width - 120 - 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("gameObject"), GUIContent.none);
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Optional objects with tag.");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUI.indentLevel = 0;
        EditorGUILayout.LabelField(new GUIContent("Pop object on hit."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultPopObjectPrefab"), GUIContent.none);

        EditorGUI.indentLevel = 1;

        list.DoLayoutList();

        EditorGUI.indentLevel = 0;

        using (var scope = new GUILayout.HorizontalScope())
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enableReflectionMax"), new GUIContent("Reflection limitation"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("reflectionMax"), GUIContent.none);
        }

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

            using (var scope = new BackgroundColorScope(addRendererButtonColor))
            {
                if (GUILayout.Button("Add Renderer") )
                {
                    beam.gameObject.AddComponent(typeList[popupIndex]);
                }
            }



            EditorGUILayout.EndVertical();

        }

        serializedObject.ApplyModifiedProperties();
    }

}

