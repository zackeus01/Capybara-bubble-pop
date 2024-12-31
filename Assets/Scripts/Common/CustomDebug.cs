using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomDebug
{
    // Log with optional color (default is white)
    public static void Log(string message, Color? color = null)
    {
        Debug.Log(FormatMessageWithColor(message, color ?? Color.white));
    }

    // Log with a specified color as a string (hex or color name)
    public static void Log(string message, string color)
    {
        string coloredMessage = $"<color={color}>{message}</color>";
        Debug.Log(coloredMessage);
    }

    // LogWarning with color support
    public static void LogWarning(string message, Color? color = null)
    {
        Debug.LogWarning(FormatMessageWithColor(message, color ?? Color.yellow));
    }

    // LogError with color support
    public static void LogError(string message, Color? color = null)
    {
        Debug.LogError(FormatMessageWithColor(message, color ?? Color.red));
    }

    // Format message with a color
    private static string FormatMessageWithColor(string message, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>";
    }
}
