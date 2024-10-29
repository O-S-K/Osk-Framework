using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class UIManager : GameFrameworkComponent
    {
        [ReadOnly, SerializeField]
        private RootUI rootUI;
        public UIMoveEffect ParticleUI => rootUI.ParticleUI;
        public Canvas GetCanvas => rootUI.GetCanvas;
        public Camera GetUICamera => rootUI.GetUICamera;
 

        public override void OnInit()
        { 
            if (rootUI == null)
            {
                rootUI = FindObjectOfType<RootUI>();
                if (rootUI != null)
                {
                    rootUI.Initialize();
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
            return rootUI.ListViews.Spawn<T>(path, isCache, isHidePrevPopup);
        }
        
        public void Delete<T>(T popup) where T : View
        {
            rootUI.ListViews.Delete<T>(popup);
        }

        public T Open<T>(bool isHidePrevPopup = false) where T : View
        {
            return rootUI.ListViews.Open<T>(isHidePrevPopup);
        }

        public T TryOpen<T>(bool isHidePrevPopup = false) where T : View
        {
            return rootUI.ListViews.TryOpen<T>(isHidePrevPopup);
        }

        public void Open(View view)
        {
            rootUI.ListViews.Open(view, true);
        }

        public void Hide(View view)
        {
            rootUI.ListViews.Hide(view);
        }

        public void HideAll()
        {
            rootUI.ListViews.HideAll();
        } 
        
        public void HideIgnore<T>() where T : View
        {
            rootUI.ListViews.HideIgnore<T>();
        }
        
        public T Get<T>() where T : View
        {
            return rootUI.ListViews.Get<T>();
        }

        public bool IsShowing(View view)
        {
            return rootUI.ListViews.Get(view).IsShowing;
        }

        public List<View> GetAll()
        {
            return rootUI.ListViews.GetAll();
        }

        #endregion
    }
}