using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class UIManager : GameFrameworkComponent
    {
        [ReadOnly, SerializeField] private RootUI _rootUI;
        public UIMoveEffect ParticleUI => _rootUI.ParticleUI;
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

        #region Popups

        public T Spawn<T>(string path, bool isCache = true, bool isHidePrevPopup = true) where T : View
        {
            return _rootUI.ListViews.Spawn<T>(path, isCache, isHidePrevPopup);
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
            _rootUI.ListViews.Open(view, true);
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

        public T Get<T>() where T : View
        {
            return _rootUI.ListViews.Get<T>();
        }

        public T GetOrOpen<T>() where T : View
        {
            var view = _rootUI.ListViews.GetIsActive<T>();
            if (view == null)
            {
                view = Open<T>();
            }
            return view;
        }

        public bool IsShowing(View view)
        {
            return _rootUI.ListViews.Get(view).IsShowing;
        }

        public List<View> GetAll()
        {
            return _rootUI.ListViews.GetAll();
        }

        #endregion
    }
}