using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class TextureUtils : MonoBehaviour
    {
        public List<Texture2D> SplitTexture(Texture2D sourceTexture)
        {
            List<Texture2D> ListTexture = new();
            int collums = 2;
            int rows = 2;
            int width = sourceTexture.width / collums;
            int height = sourceTexture.height / rows;
            int xParts = collums;
            int yParts = rows;
            for (int y = 0; y < yParts; y++)
            {
                for (int x = 0; x < xParts; x++)
                {
                    Texture2D newTexture = new(width, height);
                    Color[] pixels = sourceTexture.GetPixels(x * width, y * height, width, height);
                    newTexture.SetPixels(pixels);
                    newTexture.Apply();
                    ListTexture.Add(newTexture);
                }
            }

            return ListTexture;
        }
    }
}