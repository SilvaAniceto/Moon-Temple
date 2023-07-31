using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorScale : MonoBehaviour
{
    [SerializeField] private List<Color> PrimaryColors = new List<Color>();
    [SerializeField] private List<Image> PrimaryColorsPanel = new List<Image>();

    [SerializeField] private List<Color> SecundaryColors = new List<Color>();
    [SerializeField] private List<Image> SecundaryColorsPanel = new List<Image>();

    [SerializeField] private List<Color> TertiaryColors = new List<Color>();
    [SerializeField] private List<Image> TertiaryColorsPanel = new List<Image>();

    [SerializeField] private Image BlankSheet;
    [SerializeField] private Button ColorButton;
    [SerializeField] private Color Color1;
    [SerializeField] private Color Color2;

    private void Awake()
    {
        PrimaryColors.Clear();
        PrimaryColors.Add(Color.red);
        PrimaryColors.Add(new Color() { r = 0, g = 0.5f, b = 1, a = 1 });
        PrimaryColors.Add(new Color() { r = 0, g = 0.5f, b = 0, a = 1 });
        PrimaryColors.Add(Color.yellow);

        for (int i = 0; i < PrimaryColors.Count; i++)
        {
            PrimaryColorsPanel[i].color = PrimaryColors[i];
            if (i == 0)
            {
                Color color1 = (PrimaryColors[i] + PrimaryColors[i + 1]) / 2;
                Color color2 = (PrimaryColors[i] + PrimaryColors[PrimaryColors.Count - 1]) / 2;
                if (!SecundaryColors.Contains(color1))
                {
                    SecundaryColors.Add(color1);
                }
                if (!SecundaryColors.Contains(color2))
                {
                    SecundaryColors.Add(color2);
                }
            }
            else if (i == PrimaryColors.Count - 1)
            {
                Color color1 = (PrimaryColors[i] + PrimaryColors[0]) / 2;
                Color color2 = (PrimaryColors[i] + PrimaryColors[i - 1]) / 2;
                if (!SecundaryColors.Contains(color1))
                {
                    SecundaryColors.Add(color1);
                }
                if (!SecundaryColors.Contains(color2))
                {
                    SecundaryColors.Add(color2);
                }
            }
            else
            {
                Color color1 = (PrimaryColors[i] + PrimaryColors[i + 1]) / 2;
                Color color2 = (PrimaryColors[i] + PrimaryColors[i - 1]) / 2;
                if (!SecundaryColors.Contains(color1))
                {
                    SecundaryColors.Add(color1);
                }
                if (!SecundaryColors.Contains(color2))
                {
                    SecundaryColors.Add(color2);
                }
            }
            SecundaryColorsPanel[i].color = SecundaryColors[i];
        }

        Color1.a = 1;
        Color2.a = 1;

        ColorButton.onClick.AddListener(delegate
        {
            BlankSheet.color = (Color1 + Color2) / 2;
        });
    }
}
