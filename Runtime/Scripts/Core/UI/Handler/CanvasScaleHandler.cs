using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class CanvasScaleHandler : MonoBehaviour
    {
        private CanvasScaler canvasScaler;

        private void Awake()
        { 
            float newRatio = (float)Screen.width / Screen.height;
            canvasScaler = Main.UI.RootUI.GetCanvasScaler;
            SetMatchWidthOrHeight(newRatio > 0.65f ? 1 : 0);
        }
        
        private void SetMatchWidthOrHeight(float value)
        {
            canvasScaler.matchWidthOrHeight = value;
        }
    }
}