using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    public class UIManager : GameFrameworkComponent
    {
        [ReadOnly, SerializeField] private RootUI _rootUI;
        public UIParticle UIParticle => _rootUI.Particle;
        public Canvas GetCanvas => _rootUI.GetCanvas;
        public Camera GetUICamera => _rootUI.GetUICamera;


        public override void OnInit()
        {
            if (_rootUI == null)
            {
                _rootUI = FindObjectOfType<RootUI>();
                if (_rootUI != null)
                {
                    _rootUI.Initialize();
                }
                else
                {
                    Logg.LogError("HUD is null");
                }
            }
        }

        #region Canvas

         
        public void SetCanvas(int sortOrder = 0, string sortingLayerName = "Default", RenderMode renderMode = RenderMode.ScreenSpaceOverlay, bool pixelPerfect = false, UnityEngine.Camera camera = null)
        {
            _rootUI.GetCanvas.renderMode = renderMode;
            _rootUI.GetCanvas.sortingOrder = sortOrder;
            _rootUI.GetCanvas.sortingLayerName = sortingLayerName;
            _rootUI.GetCanvas.pixelPerfect = pixelPerfect;
            _rootUI.GetCanvas.worldCamera = camera;
        }
        
        private void SetupCanvasScalerForRatio()
        {
            float newRatio = (float)Screen.width / Screen.height;
            _rootUI.GetCanvasScaler.matchWidthOrHeight = newRatio > 0.65f ? 1 : 0;
        }
        
        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            float scaleFactor = 1f,
            float referencePixelsPerUnit = 100f)
        {
            _rootUI.GetCanvasScaler.uiScaleMode = scaleMode;
            _rootUI.GetCanvasScaler.scaleFactor = scaleFactor;
            _rootUI.GetCanvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
        }
        
        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            Vector2? referenceResolution = null,
            CanvasScaler.ScreenMatchMode screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
            float matchWidthOrHeight = 0f,
            float referencePixelsPerUnit = 100f)
        {
            _rootUI.GetCanvasScaler.uiScaleMode = scaleMode;
            _rootUI.GetCanvasScaler.referenceResolution = referenceResolution ?? new Vector2(1920, 1080);
            _rootUI.GetCanvasScaler.screenMatchMode = screenMatchMode;
            _rootUI.GetCanvasScaler.matchWidthOrHeight = matchWidthOrHeight;
            _rootUI.GetCanvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
        }
        
        public void SetCanvasScaler(
            CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize,
            Vector2? referenceResolution = null,
            CanvasScaler.ScreenMatchMode screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight,
            bool autoMatchForRatio = true,
            float referencePixelsPerUnit = 100f)
        {
            float newRatio = (float)Screen.width / Screen.height;
            SetCanvasScaler(scaleMode, referenceResolution, screenMatchMode, newRatio > 0.65f ? 1 : 0, referencePixelsPerUnit);
        }
        #endregion

        #region Views

        public T Spawn<T>(string path, bool isCache = true, bool isHidePrevPopup = true) where T : View
        {
            return _rootUI.ListViews.Spawn<T>(path, isCache, isHidePrevPopup);
        }

        public T SpawnCache<T>(T view, bool isHidePrevPopup = true) where T : View
        {
            return _rootUI.ListViews.Spawn(view, isHidePrevPopup);
        }

        public void Delete<T>(T popup) where T : View
        {
            _rootUI.ListViews.Delete<T>(popup);
        }

        public T Open<T>(bool isHidePrevPopup = false) where T : View
        {
            return _rootUI.ListViews.Open<T>(isHidePrevPopup);
        }

        public void OpenPrevious()
        {
            _rootUI.ListViews.OpenPrevious();
        }

        public T TryOpen<T>(bool isHidePrevPopup = false) where T : View
        {
            return _rootUI.ListViews.TryOpen<T>(isHidePrevPopup);
        }

        public void Open(View view)
        {
            //_rootUI.ListViews.Open(view, true);
        }

        public void Hide(View view)
        {
            _rootUI.ListViews.Hide(view);
        }

        public void HideAll()
        {
            _rootUI.ListViews.HideAll();
        }

        public void HideAllIgnoreView<T>() where T : View
        {
            _rootUI.ListViews.HideIgnore<T>();
        }

        public void HideAllIgnoreView<T>(T[] viewsToKeep) where T : View
        {
            _rootUI.ListViews.HideIgnore(viewsToKeep);
        }

        public T Get<T>(bool isInitOnScene = true) where T : View
        {
            return _rootUI.ListViews.Get<T>(isInitOnScene);
        }

        public T GetOrOpen<T>() where T : View
        {
            var view = _rootUI.ListViews.Get<T>();
            if (view == null)
            {
                if (!view.IsShowing)
                    view = Open<T>();
            }
            else
            {
                if (!view.IsShowing)
                    view.Open();
            }
            return view;
        }

        public bool IsShowing(View view)
        {
            return _rootUI.ListViews.Get<View>().IsShowing;
        }

        public List<View> GetAll(bool isInitOnScene)
        {
            return _rootUI.ListViews.GetAll(isInitOnScene);
        }

        #endregion
    }
}