using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace CustomGameController
{
    [CustomEditor(typeof(MapCreator))]
    public class MapEditor : Editor
    {
        MapCreator mapCreator = new MapCreator();

        SerializedProperty propType;

        int selectedIndex;

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

            //EditorGUI.DrawPreviewTexture(new Rect(25, 60, 100, 100), mapCreator.texture);

            GUILayout.Space(10);

            if (GUILayout.Button("Create")) mapCreator.InstatiateProp(selectedIndex);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
