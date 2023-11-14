using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapCreator))]
public class MapEditor : Editor
{
    MapCreator mapCreator = new MapCreator();

    SerializedProperty propType;

    int selectedIndex;
    Vector3 position;
    Vector3 rotation;

    private void OnEnable()
    {
        propType = serializedObject.FindProperty("propType");
        mapCreator = (MapCreator)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propType);

        GUILayout.Space(10);

        string[] id = mapCreator.CurrentProps.id.Trim().Split(',');
        selectedIndex = EditorGUILayout.Popup("Prefab", selectedIndex, id);

        GUILayout.Space(10);

        position = EditorGUILayout.Vector3Field("Position", position);
        rotation = EditorGUILayout.Vector3Field("Rotation", rotation);

        GUILayout.Space(10);

        if (GUILayout.Button("Create")) mapCreator.InstatiateProp(selectedIndex, position, rotation);

        serializedObject.ApplyModifiedProperties();
    }
}
