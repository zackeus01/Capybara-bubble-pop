using System;
using System.Collections.Generic;

[Serializable]
public enum BallColor
{
    None,
    Blue,
    Green,
    Yellow,
    Red,
    Orange,
    Violet,
    Pink,
    Cyan
}

public class BallColorConverter
{
    private static readonly Dictionary<string, BallColor> TextToColorMap = new()
    {
        { "Bl", BallColor.Blue },
        { "Gr", BallColor.Green },
        { "Ye", BallColor.Yellow },
        { "Re", BallColor.Red },
        { "Or", BallColor.Orange },
        { "Vi", BallColor.Violet },
        { "Pi", BallColor.Pink },
        { "Cy", BallColor.Cyan },
        { "No", BallColor.None}
    };

    public static BallColor TextToBallColor(string txt)
    {
        return TextToColorMap.TryGetValue(txt, out var color) ? color : BallColor.None;
    }

    public static string BallColorToText(BallColor color)
    {
        return color switch
        {
            BallColor.Blue => "Bl",
            BallColor.Green => "Gr",
            BallColor.Yellow => "Ye",
            BallColor.Red => "Re",
            BallColor.Orange => "Or",
            BallColor.Violet => "Vi",
            BallColor.Pink => "Pi",
            BallColor.Cyan => "Cy",
            BallColor.None => "No"
        };
    }
}
