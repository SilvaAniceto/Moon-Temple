using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomGameController
{
    [CustomEditor(typeof(MapCreator))]
    public class MapEditor : Editor
    {
        SerializedProperty objects;
        SerializedProperty mapProps;

        private void OnEnable()
        {
            objects = serializedObject.FindProperty("objects");
            mapProps = serializedObject.FindProperty("mapProps");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(objects);
            EditorGUILayout.PropertyField(mapProps);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
