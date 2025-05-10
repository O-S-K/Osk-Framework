using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public partial class UIManager
    {
        public void SetCanvas(int sortOrder = 0, string sortingLayerName = "Default",
            RenderMode renderMode = RenderMode.ScreenSpaceOverlay, bool pixelPerfect = false,
            UnityEngine.Camera camera = null)
        {
            RootUI.Canvas.renderMode = renderMode;
            RootUI.Canvas.sortingOrder = sortOrder;
            RootUI.Canvas.sortingLayerName = sortingLayerName;
            RootUI.Canvas.pixelPerfect = pixelPerfect;
            RootUI.Canvas.worldCamera = camera;
        }

        public void SetupCanvasScaleForRatio()
        {
            float newRatio = (float)Screen.width / Screen.height;
            RootUI.CanvasScaler.matchWidthOrHeight = newRatio > 0.65f ? 1 : 0;
        }

        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            float scaleFactor = 1f,
            float referencePixelsPerUnit = 100f)
        {
            RootUI.CanvasScaler.uiScaleMode = scaleMode;
            RootUI.CanvasScaler.scaleFactor = scaleFactor;
            RootUI.CanvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
        }

        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            Vector2? referenceResolution = null,
            CanvasScaler.ScreenMatchMode screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
            float matchWidthOrHeight = 0f,
            float referencePixelsPerUnit = 100f)
        {
            RootUI.CanvasScaler.uiScaleMode = scaleMode;
            RootUI.CanvasScaler.referenceResolution = referenceResolution ?? new Vector2(1920, 1080);
            RootUI.CanvasScaler.screenMatchMode = screenMatchMode;
            RootUI.CanvasScaler.matchWidthOrHeight = matchWidthOrHeight;
            RootUI.CanvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
        }

        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            Vector2? referenceResolution = null,
            CanvasScaler.ScreenMatchMode screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
            bool autoMatchForRatio = true,
            float referencePixelsPerUnit = 100f)
        {
            float newRatio = (float)Screen.width / Screen.height;
            SetCanvasScaler(scaleMode, referenceResolution, screenMatchMode, newRatio > 0.65f ? 1 : 0,
                referencePixelsPerUnit);
        }

        public void ShowRayCast()
        {
            var graphicRayCaster = Canvas.GetComponent<GraphicRaycaster>();
            if (graphicRayCaster != null)
                graphicRayCaster.ignoreReversedGraphics = true;
        }

        public void HideRayCast()
        {
            var graphicRayCaster = Canvas.GetComponent<GraphicRaycaster>();
            if (graphicRayCaster != null)
                graphicRayCaster.ignoreReversedGraphics = false;
        }
    }
}