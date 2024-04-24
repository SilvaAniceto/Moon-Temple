using UnityEngine;

namespace CustomGameController
{
    public static class InputHandler 
    {
        public static float GetAxisFloatValue(float value, float minValue, float maxValue)
        {
            float returnValue = Mathf.Round(Mathf.InverseLerp(minValue, maxValue, value) * 100.0f) / 100.0f;

            return returnValue;
        }

        public static Vector2 GetAxisVector2Value(Vector2 value, float minValue, float maxValue)
        {
            float x = Mathf.Round(Mathf.InverseLerp(minValue * Mathf.Round(value.x), maxValue * Mathf.Round(value.x), value.x) * 100.0f) / 100.0f;
            float y = Mathf.Round(Mathf.InverseLerp(minValue * Mathf.Round(value.y), maxValue * Mathf.Round(value.y), value.y) * 100.0f) / 100.0f;

            Vector2 returnValue = new Vector2(x, y);

            return returnValue;
        }
    }
}
