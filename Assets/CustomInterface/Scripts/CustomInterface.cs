using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomInterface
{
    //[ExecuteInEditMode]
    public class CustomInterface : MonoBehaviour
    {

        [SerializeField] private List<GameObject> m_alignmentAnchor = new List<GameObject>();
        [SerializeField] private int m_anchorRadius;
        [SerializeField] private Color m_color = Color.white;
        [SerializeField] private float m_rotationAngle;
       
        // Start is called before the first frame update
        void Start()
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

        // Update is called once per frame
        void Update()
        {
            DrawCircle(m_alignmentAnchor.Count, m_anchorRadius);

            if (Input.GetButtonDown("Submit"))
                GetComponent<RectTransform>().rotation = Quaternion.Slerp(GetComponent<RectTransform>().rotation, new Quaternion(0, 0,Mathf.Deg2Rad * m_rotationAngle, 0), 0.8f);

        }

        public void DrawCircle(int p_steps, float p_radius)
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
    }
}
