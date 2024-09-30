using System;
using UnityEngine;

public class TextUtils
{
    public TextUtils()
    {
    }

    public static string TextWith(string text, int size)
    {
        return "<size=" + size + ">" + text + "</size>";
    }

    public static string TextWith(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }

    public static string TextWith(string text, string color, int size)
    {
        return "<size=" + size + ">" + "<color=" + color + ">" + text + "</color></size>";
    }

    public static string TextWith(string text, Color color)
    {
        var hex = ColorUtility.ToHtmlStringRGB(color);
        return "<color=#" + hex + ">" + text + "</color>";
    }

    public static string TextWith(string text, Color color, int size)
    {
        var hex = ColorUtility.ToHtmlStringRGB(color);
        return "<size=" + size + ">" + "<color=#" + hex + ">" + text + "</color></size>";
    }
}
