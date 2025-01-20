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

        public RootUI RootUI
        {
            get
            {
                if (_rootUI == null)
                {
                    _rootUI = FindObjectOfType<RootUI>();
                    if (_rootUI != null)
                    {
                        _rootUI.Initialize();
                    }
                    return _rootUI;
                }
                else
                {
                    return _rootUI;
                }
            }
        }


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
            GetCanvas.GetComponent<GraphicRaycaster>().ignoreReversedGraphics = true;
        }

        public void HideRayCast()
        {
            GetCanvas.GetComponent<GraphicRaycaster>().ignoreReversedGraphics = false;
        }

        #endregion

        #region Views

        public T Spawn<T>(string path, bool isCache = true, bool isHidePrevPopup = true) where T : View
        {
            return RootUI.ListViews.Spawn<T>(path, isCache, isHidePrevPopup);
        }

        public T SpawnCache<T>(T view, bool isHidePrevPopup = true) where T : View
        {
            return RootUI.ListViews.Spawn(view, isHidePrevPopup);
        }

        public void Delete<T>(T popup) where T : View
        {
            RootUI.ListViews.Delete<T>(popup);
        }

        public T Open<T>(bool isHidePrevPopup = false) where T : View
        {
            return RootUI.ListViews.Open<T>(isHidePrevPopup);
        }

        public void OpenPrevious()
        {
            RootUI.ListViews.OpenPrevious();
        }

        public T TryOpen<T>(bool isHidePrevPopup = false) where T : View
        {
            return RootUI.ListViews.TryOpen<T>(isHidePrevPopup);
        }

        public void Open(View view)
        {
            //RootUI.ListViews.Open(view, true);
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

        public T Get<T>(bool isInitOnScene = true) where T : View
        {
            return RootUI.ListViews.Get<T>(isInitOnScene);
        }

        public T GetOrOpen<T>() where T : View
        {
            var view = RootUI.ListViews.Get<T>();
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
            return RootUI.ListViews.Get<View>().IsShowing;
        }

        public List<View> GetAll(bool isInitOnScene)
        {
            return RootUI.ListViews.GetAll(isInitOnScene);
        }

        #endregion
    }
}