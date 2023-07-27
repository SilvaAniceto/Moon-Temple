using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorScale : MonoBehaviour
{
    private float[] colorArray = new float[3];
    [Range(0, 1)] public float red;
    [Range(0, 1)] public float green;
    [Range(0, 1)] public float blue;
    [Range(0, 3)] public float colorRange;

    public float[] RGBScale
    {
        get
        {
            colorArray[0] = red;
            colorArray[1] = green;
            colorArray[2] = blue;
            return colorArray;
        }
        set
        {
            red = value[0];
            green = value[1];
            blue = value[2];
        }
    }
    //public int[] Red
    //{
    //    get
    //    {
    //        red = 255;
    //        green = 0;
    //        blue = 255;

    //        colorArray[0] = red;
    //        colorArray[1] = green;
    //        colorArray[2] = blue;
    //        return colorArray;
    //    }
    //}
    //public int[] Green
    //{
    //    get
    //    {
    //        red = 0;
    //        green = 255;
    //        blue = 0;

    //        colorArray[0] = red;
    //        colorArray[1] = green;
    //        colorArray[2] = blue;
    //        return colorArray;
    //    }
    //}
    //public int[] Blue
    //{
    //    get
    //    {
    //        red = 0;
    //        green = 0;
    //        blue = 255;

    //        colorArray[0] = red;
    //        colorArray[1] = green;
    //        colorArray[2] = blue;
    //        return colorArray;
    //    }
    //}
    //public int RedValue
    //{
    //    get
    //    {
    //        return (red * 100) / 255;
    //    }
    //    set
    //    {
    //        red = value;
    //    }
    //}
    //public int GreenValue
    //{
    //    get
    //    {
    //        return (green * 100) / 255;
    //    }
    //    set
    //    {
    //        green = value;
    //    }
    //}
    //public int BlueValue
    //{
    //    get
    //    {
    //        return (blue * 100) / 255;
    //    }
    //    set
    //    {
    //        blue = value;
    //    }
    //}

    [SerializeField] private Image BlankSheet;
    [SerializeField] private Button ColorButton;

    private void Awake()
    {
        ColorButton.onClick.AddListener(delegate
        {
            SetColor();
        });

        ChangeColor(colorRange);
    }

    private void Update()
    {
        ChangeColor(colorRange );
    }

    public void SetColor()
    {
        Color newColor = new Color();
        newColor.r = red;
        newColor.g = green;
        newColor.b = blue;
        newColor.a = 100f;

        BlankSheet.color = newColor;
    }

    public void ChangeColor(float value)
    {
        if (value <= 1) red = Mathf.Clamp(value, 0, 1);
        else if (value > 1 && value <= 2) green = Mathf.Clamp(value - 1, 0, 1);
        else blue = Mathf.Clamp(value - 2, 0, 1);

        Color newColor = new Color();
        newColor.r = red;
        newColor.g = green;
        newColor.b = blue;
        newColor.a = 100f;

        BlankSheet.color = newColor;
    }
}
