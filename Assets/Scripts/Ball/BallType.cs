using System;
using System.Collections.Generic;

[Serializable]
public enum BallType
{
    Normal,
    Bomb,
    Firework,
    Rainbow,
    Ziczac,
    Blast,
    Rock,
    Lightning,
    Web,
    Grass
}

public class BallTypeConverter
{
    // Map for string to BallType
    private static readonly Dictionary<string, BallType> TextToTypeMap = new()
    {
        { "No", BallType.Normal },
        { "Bo", BallType.Bomb },
        { "Fi", BallType.Firework },
        { "Ra", BallType.Rainbow },
        { "Zi", BallType.Ziczac },
        { "Bl", BallType.Blast },
        { "Ro", BallType.Rock },
        { "Li", BallType.Lightning },
        { "We", BallType.Web },
        { "Gr", BallType.Grass }
    };

    // Convert from string to BallType
    public static BallType TextToBallType(string txt)
    {
        return TextToTypeMap.TryGetValue(txt, out var type) ? type : BallType.Normal;
    }

    // Convert from BallType to string
    public static string BallTypeToText(BallType type)
    {
        return type switch
        {
            BallType.Normal => "No",
            BallType.Bomb => "Bo",
            BallType.Firework => "Fi",
            BallType.Rainbow => "Ra",
            BallType.Ziczac => "Zi",
            BallType.Blast => "Bl",
            BallType.Rock => "Ro",
            BallType.Lightning => "Li",
            BallType.Web => "We",
            BallType.Grass => "Gr",
            _ => "None"
        };
    }
}