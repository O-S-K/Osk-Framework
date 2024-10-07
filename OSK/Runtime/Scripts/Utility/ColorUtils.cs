using UnityEngine;


namespace OSK
{
    public enum ColorCustom
    {
        White,
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
        Cyan,
        Magenta,
        Black,
        Gray,
        Brown,
        Lime,
        Pink,
        Teal,
        Indigo,
        Violet
    }

    public static class ColorUtils
    {
        public static Color GetColor(ColorCustom colorCustom)
        {
            switch (colorCustom)
            {
                case ColorCustom.Red:
                    return Color.red;
                case ColorCustom.Green:
                    return Color.green;
                case ColorCustom.Blue:
                    return Color.blue;
                case ColorCustom.Yellow:
                    return Color.yellow;
                case ColorCustom.Orange:
                    return new Color(1f, 0.5f, 0f); // Màu Cam
                case ColorCustom.Purple:
                    return new Color(0.5f, 0f, 0.5f); // Màu Tím
                case ColorCustom.Cyan:
                    return Color.cyan;
                case ColorCustom.Magenta:
                    return Color.magenta;
                case ColorCustom.Black:
                    return Color.black;
                case ColorCustom.White:
                    return Color.white;
                case ColorCustom.Gray:
                    return Color.gray;
                case ColorCustom.Brown:
                    return new Color(0.5f, 0.25f, 0f); // Màu Nâu
                case ColorCustom.Lime:
                    return new Color(0.5f, 1f, 0f); // Màu Xanh Chanh
                case ColorCustom.Pink:
                    return new Color(1f, 0.75f, 0.8f); // Màu Hồng
                case ColorCustom.Teal:
                    return new Color(0f, 0.5f, 0.5f); // Màu Teal
                case ColorCustom.Indigo:
                    return new Color(0.29f, 0f, 0.51f); // Màu Indigo
                case ColorCustom.Violet:
                    return new Color(0.5f, 0.0f, 0.5f); // Màu Violet
                default:
                    return Color.clear; // Trả về màu trong suốt nếu không có màu nào được chỉ định
            }
        }


        public static string GetColorHTML(this string str, ColorCustom clr)
        {
            Color color = ColorUtils.GetColor(clr);
            string htmlColor = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{htmlColor}>{str}</color>";
        }
    }
}