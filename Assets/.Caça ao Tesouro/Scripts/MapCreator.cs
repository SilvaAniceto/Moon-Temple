using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour
{
    #region CLASSES & ENUMS REGION
    public class Props
    {
        public List<GameObject> propsPrefab = new List<GameObject>();
        public string id = "";

        public List<GameObject> parentList = new List<GameObject>();
        public string parentId = "";
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
    #endregion

    #region SERIALIZED FIELDS
    [SerializeField] private PropsType propType = PropsType.None;
    #endregion

    #region PRIVATE FIELDS
    private static PropsType _type;
    private static int _selectedPrebabIndex;
    private static int _selectedParentIndex;

    private static string ProjectResourceFolder
    {
        get
        {
            return "Assets/Caça ao Tesouro/Resources/";
        }
    }
    private static string PrefabResourceFolder
    {
        get
        {
            return "Prefab/";
        }
    }
    private static string ThumbResourceFolder
    {
        get
        {
            return "Textures/Custom/";
        }
    }
    private static string PropFolder
    {
        get
        {
            return Type.ToString();
        }
    }
    #endregion

    #region PUBLIC FIELDS
    public static Props CurrentProps = new Props();
    public static GameObject ThisObject;
    public static GameObject CurrentPrefab;
    public static Texture2D thumb;

    public static PropsType Type 
    {
        get
        {
            return _type;
        }
        set
        {
            if (_type == value) return;

            _type = value;

            GetPropsFromFolder(_type.ToString());
        }
    }
    public static int SelectedPrebabIndex
    {
        get
        {
            return _selectedPrebabIndex;
        }

        set
        {
            if (_selectedPrebabIndex == value) return;

            _selectedPrebabIndex = value;

            GetPropThumb(Type.ToString(), _selectedPrebabIndex);
        }
    }
    #endregion

    #region PUBLIC METHODS
    [MenuItem("Map Creator/Save Thumbnail")]
    public static void SaveThumbnail()
    {
        GameObject target = null;

        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject != ThisObject)
                target = Selection.activeGameObject;
        }

        if (target == null)
        {
            if (CurrentPrefab != null)
                target = CurrentPrefab;
            else
            { 
                Debug.LogError("You need to select a GameObject in the hierarchy.");
                return;
            }
        }

        Texture2D tex = GetPNGTexture(Selection.activeTransform.gameObject);
        byte[] textureBytes = tex.EncodeToPNG();

        string path = Path.Combine(ProjectResourceFolder + ThumbResourceFolder + PropFolder.ToString(), target.name + ".png");

        File.WriteAllBytes(path, textureBytes);

        Debug.Log("Thumbnail '" + target.name + ".png' saved on path: " + path);

        AssetDatabase.Refresh();
    }
    public static void InstatiateProp(int index, string propName, Vector3 position, Vector3 rotation, bool newParent, string parentName, bool setParent, int indexParent)
    {
        if (index < 0 || index > CurrentProps.propsPrefab.Count || CurrentProps.propsPrefab.Count == 0) return;

        CurrentPrefab = null;

        CurrentPrefab = PrefabUtility.InstantiatePrefab(CurrentProps.propsPrefab[index]) as GameObject;

        GameObject parent = null;

        if (newParent)
        {
            parent = new GameObject(string.IsNullOrEmpty(parentName) ? "GameObject" : parentName);
            parent.transform.localPosition = Vector3.zero;
            parent.transform.localEulerAngles = Vector3.zero;
            parent.tag = "Environment";

            CurrentPrefab.transform.SetParent(parent.transform);
        }

        if (setParent)
        {
            parent = CurrentProps.parentList[indexParent];

            CurrentPrefab.transform.SetParent(parent.transform);
        }

        CurrentPrefab.transform.localPosition = position;
        CurrentPrefab.transform.localEulerAngles = rotation;
        CurrentPrefab.tag = "Environment";

        if (!string.IsNullOrEmpty(propName)) CurrentPrefab.name = propName;

        Selection.activeGameObject = CurrentPrefab;
    }
    public static void DestroyProp()
    {
        GameObject obj = CurrentPrefab;
        DestroyImmediate(obj);
        CurrentPrefab = null;
    }  
    public static bool SetParentList()
    {
        CurrentProps.parentList.Clear();
        CurrentProps.parentId = "";

        GameObject[] array = GameObject.FindGameObjectsWithTag("Environment");

        foreach (GameObject obj in array)
        {
            if (!CurrentProps.parentList.Contains(obj))
            {
                CurrentProps.parentList.Add(obj);
                CurrentProps.parentId += obj.name + obj.GetInstanceID() + "/";
            }
        }
        return CurrentProps.parentList.Count > 0 ? true : false;
    }
    #endregion

    #region PRIVATE METHODS
    private static void GetPropsFromFolder(string propFolder)
    {
        CurrentProps.propsPrefab.Clear();
        CurrentProps.id = "";

        string propPath = Type == PropsType.Vehicles ? propFolder + "/Vehicle with Separated Wheels" : propFolder;

        GameObject[] prefabs = Resources.LoadAll<GameObject>(PrefabResourceFolder + propPath);
        foreach (GameObject prefab in prefabs)
        {
            if (!CurrentProps.propsPrefab.Contains(prefab))
            {
                CurrentProps.propsPrefab.Add(prefab);
                CurrentProps.id += prefab.name + "/";
            }
        }
        GetPropThumb(Type.ToString(), SelectedPrebabIndex);
    }
    private static void GetPropThumb(string propFolder, int selectedIndex)
    {
        if (selectedIndex < 0 || selectedIndex > CurrentProps.propsPrefab.Count || CurrentProps.propsPrefab.Count  == 0) return;

        thumb = Resources.Load<Texture2D>(ThumbResourceFolder + propFolder + "/" + CurrentProps.propsPrefab[selectedIndex].name);
    }
    private static Texture2D GetPNGTexture(GameObject target, int resWidth = 180, int resHeight = 180, bool transparent = true)
    {
        GameObject o = new GameObject("~TempCam");
        Camera cam = o.AddComponent<Camera>();
        cam.cullingMask = 2;
        cam.aspect = (float)resHeight / (float)resWidth;

        Dictionary<GameObject, int> originalLayers = new Dictionary<GameObject, int>();
        Transform[] allTransforms = target.GetComponentsInChildren<Transform>();
        foreach (Transform t in allTransforms)
        {
            originalLayers.Add(t.gameObject, t.gameObject.layer);
            t.gameObject.layer = 1;
        }

        Bounds bounds = new Bounds(target.transform.position, Vector3.zero);
        foreach (Renderer renderer in target.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        cam.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        cam.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;

        var tex_white = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        var tex_black = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        var tex_final = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        
        var render_texture = RenderTexture.GetTemporary(resWidth, resHeight, 24, RenderTextureFormat.ARGB32);
        var grab_area = new Rect(0, 0, resWidth, resHeight);

        RenderTexture.active = render_texture;
        cam.targetTexture = render_texture;
        cam.clearFlags = CameraClearFlags.SolidColor;

        if (transparent)
        {
            cam.backgroundColor = Color.black;
            cam.Render();
            tex_black.ReadPixels(grab_area, 0, 0);
            tex_black.Apply();

            cam.backgroundColor = Color.white;
            cam.Render();
            tex_white.ReadPixels(grab_area, 0, 0);
            tex_white.Apply();

            for (int y = 0; y < tex_final.height; ++y)
            {
                for (int x = 0; x < tex_final.width; ++x)
                {
                    float alpha = tex_white.GetPixel(x, y).r - tex_black.GetPixel(x, y).r;
                    alpha = 1.0f - alpha;
                    Color color;
                    if (alpha == 0)
                    {
                        color = Color.clear;
                    }
                    else
                    {
                        color = tex_black.GetPixel(x, y) / alpha;
                    }
                    color.a = alpha;
                    tex_final.SetPixel(x, y, color);
                }
            }
        }
        else
        {
            cam.backgroundColor = Color.white;
            cam.Render();
            tex_final.ReadPixels(grab_area, 0, 0);
            tex_final.Apply();
        }

        foreach (KeyValuePair<GameObject, int> entry in originalLayers)
        {
            entry.Key.layer = entry.Value;
        }

        RenderTexture.ReleaseTemporary(render_texture);
        DestroyImmediate(cam.gameObject);

        Texture2D.DestroyImmediate(tex_black);
        Texture2D.DestroyImmediate(tex_white);
        return tex_final;
    }
    #endregion
}
