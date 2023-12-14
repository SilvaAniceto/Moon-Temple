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
    SerializedProperty propType;

    string propName;
    int selectedIndex;
    int parentIndex;
    Vector3 position;
    Vector3 rotation;
    bool setParent;
    bool newParent;
    string parentName;

    private void OnEnable()
    {
        propType = serializedObject.FindProperty("propType");
        MapCreator.ThisObject = target.GameObject();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(propType);

        MapCreator.Type = (MapCreator.PropsType)propType.intValue;

        GUILayout.Space(10);

        string[] id = MapCreator.CurrentProps.id.Trim().Split('/', System.StringSplitOptions.RemoveEmptyEntries);
        selectedIndex = EditorGUILayout.Popup("Prefab", selectedIndex, id);

        MapCreator.SelectedPrebabIndex = selectedIndex;

        GUILayout.Space(10);

        GUI.DrawTexture(new Rect(100, 80, 180, 180), MapCreator.thumb, ScaleMode.ScaleToFit);

        GUILayout.Space(220);

        newParent = EditorGUILayout.ToggleLeft("Create New Parent", newParent);

        if (newParent)
        {
            setParent = false;
            parentName = EditorGUILayout.TextField("Parent Name", parentName);
        }

        if (MapCreator.SetParentList() && !newParent)
        {
            setParent = EditorGUILayout.ToggleLeft("Nest In Parent", setParent);
            if (setParent)
            {
                string[] parentId = MapCreator.CurrentProps.parentId.Trim().Split('/', System.StringSplitOptions.RemoveEmptyEntries);
                parentIndex = EditorGUILayout.Popup("Parent", parentIndex, parentId);
            }
        }

        GUILayout.Space(30);

        propName = EditorGUILayout.TextField("Prop Name", propName);
        position = EditorGUILayout.Vector3Field("Position", position);
        rotation = EditorGUILayout.Vector3Field("Rotation", rotation);

        GUILayout.Space(10);

        if (GUILayout.Button("Create Prop"))
        {
            MapCreator.InstatiateProp(selectedIndex, propName, position, rotation, newParent, parentName, setParent, parentIndex);
            propName = "";
            parentName = "";
            newParent = false;
            setParent = false;
        }

        if (MapCreator.CurrentPrefab != null)
            if (GUILayout.Button("Delete Prop"))
                MapCreator.DestroyProp();

        //if (GUILayout.Button("Save Thumbnail")) MapCreator.SaveThumbnail();

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
