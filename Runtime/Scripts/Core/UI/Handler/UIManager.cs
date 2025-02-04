using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace OSK
{
    public class UIManager : GameFrameworkComponent
    {
        [ReadOnly, SerializeField] private RootUI _rootUI;
        public UIParticle UIParticle => _rootUI.Particle;
        public Canvas GetCanvas => _rootUI.GetCanvas;
        public Camera GetUICamera => _rootUI.GetUICamera;

        public RootUI RootUI
        {
            get
            {
                if (_rootUI != null)
                    return _rootUI;

                _rootUI = FindObjectOfType<RootUI>();
                if (_rootUI != null)
                    _rootUI.Initialize();
                return _rootUI;
            }
        }


        public override void OnInit()
        {
            if (_rootUI != null)
                return;

            _rootUI = FindObjectOfType<RootUI>();
            if (_rootUI != null)
                _rootUI.Initialize();
        }

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

        #region Views

        public T Spawn<T>(string path, object[] data = null, bool isCache = true, bool isHidePrevPopup = false)
            where T : View
        {
            return RootUI.ListViews.Spawn<T>(path, data, isCache, isHidePrevPopup);
        }

        public T SpawnCache<T>(T view, object[] data = null, bool isHidePrevPopup = false) where T : View
        {
            return RootUI.ListViews.Spawn(view, data, isHidePrevPopup);
        }

        public T Open<T>(object[] data = null, bool isHidePrevPopup = false) where T : View
        {
            return RootUI.ListViews.Open<T>(data, isHidePrevPopup);
        }

        public void OpenPrevious()
        {
            RootUI.ListViews.OpenPrevious();
        }

        public T TryOpen<T>(object[] data = null, bool isHidePrevPopup = false) where T : View
        {
            return RootUI.ListViews.TryOpen<T>(data, isHidePrevPopup);
        }

        public void Open(View view, object[] data = null, bool isHidePrevPopup = false)
        {
            RootUI.ListViews.Open(view, data, isHidePrevPopup);
        }

        public AlertView OpenAlert<T>(AlertSetup setup) where T : AlertView
        {
           return RootUI.ListViews.OpenAlert<T>(setup);
        } 
        
        public void Hide(View view)
        {
            RootUI.ListViews.Hide(view);
        }

        public void HideAll()
        {
            RootUI.ListViews.HideAll();
        }

        public void HideAllIgnoreView<T>() where T : View
        {
            RootUI.ListViews.HideIgnore<T>();
        }

        public void HideAllIgnoreView<T>(T[] viewsToKeep) where T : View
        {
            RootUI.ListViews.HideIgnore(viewsToKeep);
        }

        public void Delete<T>(T popup) where T : View
        {
            RootUI.ListViews.Delete<T>(popup);
        }


        public T Get<T>(bool isInitOnScene = true) where T : View
        {
            return RootUI.ListViews.Get<T>(isInitOnScene);
        }

        public T GetOrOpen<T>(object[] data = null, bool hidePrevView = false) where T : View
        {
            var view = RootUI.ListViews.Get<T>();
            if (view == null)
            {
                if (!view.IsShowing)
                    view = Open<T>(data, hidePrevView);
            }
            else
            {
                if (!view.IsShowing)
                    view.Open(data);
            }

            return view;
        }

        public bool IsShowing(View view)
        {
            return RootUI.ListViews.Get<View>().IsShowing;
        }

        public List<View> GetAll(bool isInitOnScene)
        {
            return RootUI.ListViews.GetAll(isInitOnScene);
        }

        #endregion
    }
}