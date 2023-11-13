using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CustomGameController
{
    public enum MapProps
    {
        None,
        Buildings,
        Natures,
        Props,
        Roads,
        Vehicles
    }

    public class MapCreator : MonoBehaviour
    {
        [SerializeField] public List<GameObject> objects = new List<GameObject>();
        [SerializeField] private string PropsFolder = "Assets/SimplePoly City - Low Poly Assets/Prefab";

        [SerializeField] private MapProps mapProps = MapProps.None;
        void Start()
        {
        
        }

        void Update()
        {
            Debug.Log(mapProps);
            Debug.Log(objects.Count);
        }
    }
}
