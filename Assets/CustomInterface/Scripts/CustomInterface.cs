using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace CustomInterface
{
    [ExecuteInEditMode]
    public class CustomInterface : MonoBehaviour
    {

        [SerializeField] private List<GameObject> m_alignmentAnchor = new List<GameObject>();
        [SerializeField] private Color m_color = Color.white;
        [SerializeField] private float m_rotationAngle;
        [SerializeField] private int m_radius;

        private int radius;
        private int anchorCount;
        public int AnchorCount
        {
            get 
            { 
                return anchorCount; 
            }

            private set
            {
                if (anchorCount == value)
                    return;

                anchorCount = value;

                DestroyObjects();

                if (anchorCount >= 3)
                    ArrangeGameObjects(true);
            }
        }

        public int Radius
        {
            get
            {
                return radius;
            }

            private set
            {
                if (radius == value)
                    return;

                radius = value;

                ArrangeGameObjects(false);
            }
        }
       
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            AnchorCount = m_alignmentAnchor.Count;
            Radius = m_radius;
        }

        public void SetPoligonalVertex(int p_steps, float p_radius)
        {
            for (int currentStep = 0; currentStep < p_steps; currentStep++)
            {
                float circunferenceProgress = (float)currentStep / p_steps;

                float currentRadian = circunferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                float x = xScaled * p_radius;
                float y = yScaled * p_radius;

                m_alignmentAnchor[currentStep].GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
            }
        }

        public void ArrangeGameObjects(bool p_newObjects)
        {
            if (p_newObjects)
            {
                for (int i = 0; i < m_alignmentAnchor.Count; i++)
                {   
                    GameObject obj = new GameObject("Anchor", typeof(RectTransform));

                    m_alignmentAnchor[i] = obj;

                    m_alignmentAnchor[i].transform.SetParent(transform);

                    m_alignmentAnchor[i].AddComponent<Image>();

                    m_alignmentAnchor[i].GetComponent<Image>().color = m_color;

                    m_alignmentAnchor[i].GetComponent<RectTransform>().localPosition = Vector3.zero;

                    m_alignmentAnchor[i].GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;
                }

                m_rotationAngle = 360 / m_alignmentAnchor.Count;
            }

            SetPoligonalVertex(AnchorCount, Radius);
        }

        void DestroyObjects()
        {
            if (transform.childCount == 0) return;

            int childCount = transform.childCount;
            for (int i = childCount-1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject, false);
            }
        }
    }
}
