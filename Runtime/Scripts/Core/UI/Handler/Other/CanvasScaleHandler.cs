using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public partial class UIManager
    {
        #region Canvas

        public void SetCanvas(int sortOrder = 0, string sortingLayerName = "Default",
            RenderMode renderMode = RenderMode.ScreenSpaceOverlay, bool pixelPerfect = false,
            UnityEngine.Camera camera = null)
        {
            RootUI.GetCanvas.renderMode = renderMode;
            RootUI.GetCanvas.sortingOrder = sortOrder;
            RootUI.GetCanvas.sortingLayerName = sortingLayerName;
            RootUI.GetCanvas.pixelPerfect = pixelPerfect;
            RootUI.GetCanvas.worldCamera = camera;
        }

        public void SetupCanvasScaleForRatio()
        {
            float newRatio = (float)Screen.width / Screen.height;
            RootUI.GetCanvasScaler.matchWidthOrHeight = newRatio > 0.65f ? 1 : 0;
        }

        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            float scaleFactor = 1f,
            float referencePixelsPerUnit = 100f)
        {
            RootUI.GetCanvasScaler.uiScaleMode = scaleMode;
            RootUI.GetCanvasScaler.scaleFactor = scaleFactor;
            RootUI.GetCanvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
        }

        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            Vector2? referenceResolution = null,
            CanvasScaler.ScreenMatchMode screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
            float matchWidthOrHeight = 0f,
            float referencePixelsPerUnit = 100f)
        {
            RootUI.GetCanvasScaler.uiScaleMode = scaleMode;
            RootUI.GetCanvasScaler.referenceResolution = referenceResolution ?? new Vector2(1920, 1080);
            RootUI.GetCanvasScaler.screenMatchMode = screenMatchMode;
            RootUI.GetCanvasScaler.matchWidthOrHeight = matchWidthOrHeight;
            RootUI.GetCanvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
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
            var graphicRayCaster = GetCanvas.GetComponent<GraphicRaycaster>();
            if (graphicRayCaster != null)
                graphicRayCaster.ignoreReversedGraphics = true;
        }

        public void HideRayCast()
        {
            var graphicRayCaster = GetCanvas.GetComponent<GraphicRaycaster>();
            if (graphicRayCaster != null)
                graphicRayCaster.ignoreReversedGraphics = false;
        }

        #endregion
    }
}