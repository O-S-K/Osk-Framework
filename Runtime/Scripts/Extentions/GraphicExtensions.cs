using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public static class GraphicExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Color WithA(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }

        public static void SetColorHex(this UnityEngine.UI.Graphic graphic, string hex)
        {
            Color color = ColorUtility.TryParseHtmlString(hex, out color) ? color : Color.white;
            graphic.color = color;
        }

        public static void SetTextMeshFade(this TextMeshProUGUI text, float value)
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        }


        public static T ChangeAlpha<T>(this T g, float newAlpha) where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
        public static void SetTextFade(this Text text, float value)
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        }

        public static void SetTextFade(this TMPro.TextMeshProUGUI text, float value)
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        }

        public static void SetImageFade(this UnityEngine.UI.Image image, float value)
        {
            Color color = image.color;
            color.a = value;
            image.color = color;
        }

        public static void SetCanvasGroupFade(this CanvasGroup canvasGroup, float value)
        {
            canvasGroup.alpha = value;
        }


        public static void SetColorMaterial(this Material material, string nameID, Color color)
        {
            var nameId = Shader.PropertyToID(nameID);
            material.SetColor(nameId, color);
        }

        public static void SetColorMaterials(this Material[] materials, string nameID, Color color)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor(Shader.PropertyToID(nameID), color);
            }
        }
    }
}