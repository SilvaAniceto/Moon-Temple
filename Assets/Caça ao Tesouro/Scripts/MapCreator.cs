using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace CustomGameController
{
    

    [ExecuteInEditMode]
    public class MapCreator : MonoBehaviour
    {
        [System.Serializable] public class Props
        {
            public List<GameObject> propsPrefab = new List<GameObject>();
            public string id = "";
        }
        public enum PropsType
        {
            None,
            Buildings,
            Natures,
            Props,
            Roads,
            Vehicles
        }

        PropsType _type;
        PropsType Type 
        {
            get
            {
                return propType;
            }
            set
            {
                if (_type == value) return;

                _type = value;

                GetPropsFromFolder(_type.ToString());
            }
        }

        [SerializeField] private PropsType propType = PropsType.None;
        
        private string PrefabResourceFolder = "Prefab/";
        private string ThumbResourceFolder = "Textures/Custom/";

        public Texture2D texture;
        public Props CurrentProps = new Props();

        private void Update()
        {
            Type = propType;
        }

        void GetPropsFromFolder(string propFolder)
        {
            CurrentProps.propsPrefab.Clear();
            CurrentProps.id = "";

            GameObject[] prefabs = Resources.LoadAll<GameObject>(PrefabResourceFolder + propFolder);
            
            foreach (GameObject prefab in prefabs)
            {
                if (!CurrentProps.propsPrefab.Contains(prefab))
                {
                    CurrentProps.propsPrefab.Add(prefab);
                    CurrentProps.id += prefab.name + ",";
                }
            }
        }

        public void InstatiateProp(int index)
        {
            if (CurrentProps.propsPrefab == null) return;

            PrefabUtility.InstantiatePrefab(CurrentProps.propsPrefab[index]);
        }

        void SaveCameraView(Camera cam)
        {
            RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
            cam.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            cam.Render();
            Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
            renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            RenderTexture.active = null;
            byte[] byteArray = renderedTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/cameracapture.png", byteArray);
        }
    }
}
