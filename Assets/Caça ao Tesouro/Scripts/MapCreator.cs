using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

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

    static PropsType Prop = PropsType.None;
    static string ProjectResourceFolder
    {
        get
        {
            return "Assets/Caça ao Tesouro/Resources/";
        }
    }
    static string PrefabResourceFolder
    {
        get
        {
            return "Prefab/";
        }
    }
    static string ThumbResourceFolder
    {
        get
        {
            return "Textures/Custom/";
        }
    }
    static string PropFolder
    {
        get
        {
            return Prop.ToString();
        }
    }

    public Texture2D texture;
    public Props CurrentProps = new Props();

    [SerializeField] private PropsType propType = PropsType.None;

    private void Update()
    {
        Type = propType;
        Prop = propType;
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

    public void InstatiateProp(int index, Vector3 position, Vector3 rotation)
    {
        if (CurrentProps.propsPrefab == null) return;

        GameObject obj = PrefabUtility.InstantiatePrefab(CurrentProps.propsPrefab[index]) as GameObject;

        obj.transform.localPosition = position;
        obj.transform.localEulerAngles = rotation;
    }

    [MenuItem("Map Creator/Save Thumbnail")]
    static void SaveThumbnail()
    {
        GameObject target = null;

        if (Selection.activeTransform != null)
        {
            target = Selection.activeTransform.gameObject;
        }

        if (target == null)
        {
            Debug.LogError("You need to select a GameObject in the hierarchy.");
            return;
        }

        Texture2D tex = GetPNGTexture(Selection.activeTransform.gameObject);
        byte[] textureBytes = tex.EncodeToPNG();

        string path = Path.Combine(ProjectResourceFolder + ThumbResourceFolder + PropFolder.ToString(), target.name + ".png");

        File.WriteAllBytes(path, textureBytes);
    }

    public static Texture2D GetPNGTexture(GameObject target, int resWidth = 180, int resHeight = 180, bool transparent = true)
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
}
