using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorScale : MonoBehaviour
{
    [System.Serializable]
    public struct PrimaryColor
    {
        [Range(0, 1)] public float R_Parameter;
        [Range(0, 1)] public float G_Parameter;
        [Range(0, 1)] public float B_Parameter;
        [Range(0, 1)] public float A_Parameter;
    }

    [System.Serializable]
    public struct SecundaryColor
    {
        [Range(0, 1)] public float R_Parameter;
        [Range(0, 1)] public float G_Parameter;
        [Range(0, 1)] public float B_Parameter;
        [Range(0, 1)] public float A_Parameter;
    }

    PrimaryColor Red =    new PrimaryColor() { R_Parameter = 1, G_Parameter = 0, B_Parameter = 0, A_Parameter = 1 };
    PrimaryColor Yellow = new PrimaryColor() { R_Parameter = 1, G_Parameter = 1, B_Parameter = 0, A_Parameter = 1 };
    PrimaryColor Blue =   new PrimaryColor() { R_Parameter = 0, G_Parameter = 0, B_Parameter = 1, A_Parameter = 1 };
    PrimaryColor Green =  new PrimaryColor() { R_Parameter = 0, G_Parameter = 1, B_Parameter = 0, A_Parameter = 1 };

    public List<PrimaryColor> PrimaryColors = new List<PrimaryColor>();

    SecundaryColor Orange = new SecundaryColor() { R_Parameter = 1, G_Parameter = 0.5f, B_Parameter = 1, A_Parameter = 1 };
    SecundaryColor Violet = new SecundaryColor() { R_Parameter = 0.5f, G_Parameter = 0, B_Parameter = 1, A_Parameter = 1 };

    public List<SecundaryColor> SecundaryColors = new List<SecundaryColor>();

    [SerializeField] private Image Color1;
    [SerializeField] private Image Color2;
    [SerializeField] private Image Color3;
    [SerializeField] private Image BlankSheet;
    [SerializeField] private Button ColorButton;

    private void Awake()
    {
        PrimaryColors.Add(Red);
        PrimaryColors.Add(Yellow);
        PrimaryColors.Add(Blue);
        PrimaryColors.Add(Green);

        SecundaryColors.Add(Orange);
        SecundaryColors.Add(Violet);

        ColorButton.onClick.AddListener(delegate
        {
            Color color1 = new Color();
            color1.r = Red.R_Parameter;
            color1.g = Red.G_Parameter;
            color1.b = Red.B_Parameter;
            color1.a = Red.A_Parameter;
            Color1.color = color1;

            Color color2 = new Color();
            color2.r = Blue.R_Parameter;
            color2.g = Blue.G_Parameter;
            color2.b = Blue.B_Parameter;
            color2.a = Blue.A_Parameter;
            Color2.color = color2;

            Color color3 = new Color();
            color3.r = Yellow.R_Parameter;
            color3.g = Yellow.G_Parameter;
            color3.b = Yellow.B_Parameter;
            color3.a = Yellow.A_Parameter;
            Color3.color = color3;

            BlankSheet.color = (Color.yellow + Color.blue) / 2;
        });
    }
}
