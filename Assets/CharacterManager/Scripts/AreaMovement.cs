using UnityEngine;

namespace CharacterManager
{
    public class AreaMovement : MonoBehaviour
    {
        public static AreaMovement m_areaMovementInstance;

        private LineRenderer m_lineRenderer;

        private void Awake()
        {
            if (m_areaMovementInstance == null)
                m_areaMovementInstance = this;

            m_lineRenderer = GetComponent<LineRenderer>();
        }

        public void DrawCircle(int p_steps, float p_radius, Vector3 p_position)
        {
            m_lineRenderer.positionCount = p_steps;

            for (int currentStep = 0; currentStep < p_steps; currentStep++)
            {
                float circunferenceProgress = (float)currentStep / p_steps;

                float currentRadian = circunferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float zScaled = Mathf.Sin(currentRadian);

                float x = xScaled * p_radius;
                float z = zScaled * p_radius;

                Vector3 currentPosition = new Vector3(p_position.x + x, transform.position.y, p_position.z + z);

                m_lineRenderer.SetPosition(currentStep, currentPosition);
            }
        }
    }
}
