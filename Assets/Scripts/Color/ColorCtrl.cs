using UnityEngine;

public static class ColorCtrl 
{
    public static Color GetColor(BallColor ballColor)
    {
        switch (ballColor)
        {
            case BallColor.Cyan:
                {
                    return GetColorFromHex("#3169eb");
                }
            case BallColor.Green:
                {
                    return GetColorFromHex("#68b138");
                }
            case BallColor.Blue:
                {
                    return GetColorFromHex("#73d9e3");
                }
            case BallColor.Orange:
                {
                    return GetColorFromHex("#ff7f10");
                }
            case BallColor.Red:
                {
                    return GetColorFromHex("#e5473e");
                }
            case BallColor.Pink:
                {
                    return GetColorFromHex("#fd4aeb");
                }
            case BallColor.Violet:
                {
                    return GetColorFromHex("#d517ff");
                }
            case BallColor.Yellow:
                {
                    return GetColorFromHex("#fbc910");
                }
            default: 
                { 
                    return Color.white;
                }
        }
    }

    private static Color GetColorFromHex(string hex)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            return Color.white;
        }
    }

    public static Gradient ConvertColorToGradient(Color color)
    {
        Gradient gradient = new Gradient();

        // Create color keys with the same color at both start and end
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = color;
        colorKeys[0].time = 0f;
        colorKeys[1].color = color;
        colorKeys[1].time = 1f;

        // Create alpha keys with the same alpha at both start and end
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = color.a;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = color.a;
        alphaKeys[1].time = 1f;

        // Assign the color and alpha keys to the gradient
        gradient.SetKeys(colorKeys, alphaKeys);

        return gradient;
    }
}
