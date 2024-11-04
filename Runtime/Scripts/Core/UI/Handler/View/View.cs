using System;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public enum EViewType
    {
        None,
        Popup,
        Overlay,
        Screen
    }

    [RequireComponent(typeof(UITransition))]
    public class View : MonoBehaviour
    {
        [Header("View")] public EViewType viewType = EViewType.Popup;
        public int depth;
        public bool isAddToViewManager = true;
        public bool isPreloadSpawn = true;

        [ReadOnly] public bool isInitOnScene = false;
        public bool IsShowing => _isShowing;
        [ShowInInspector, ReadOnly] private bool _isShowing;

        protected UITransition _uiTransition;
        protected ViewManager _viewManager;

        public Action EventAfterInit { get; set; }
        public Action EventBeforeOpened { get; set; }
        public Action EventAfterOpened { get; set; }
        public Action EventBeforeClosed { get; set; }
        public Action EventAfterClosed { get; set; }

        public virtual void Initialize(ViewManager viewManager)
        {
            isInitOnScene = true;
            _viewManager = viewManager;
            _uiTransition = GetComponent<UITransition>();
            _uiTransition.Initialize();

            if (_viewManager == null)
            {
                Logg.LogError("View Manager is still null after initialization.");
            }
            else
            {
                Logg.Log("View Manager initialized successfully.");
            }
    
            EventAfterInit?.Invoke();
        }

        public void SetSetSiblingIndex(int index)
        {
            transform.SetSiblingIndex(index);
        }

        public void SetOderInLayer(int order)
        {
            var canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = order;
            }
        }

        public virtual void Open(object data = null)
        {
            if (_uiTransition == null)
            {
                Logg.LogError("UI Transition is null. Ensure that the View has been initialized before calling Open.");
                return;
            }

            if (_viewManager == null)
            {
                Logg.LogError("View Manager is null. Ensure that the View has been initialized before calling Open.");
                return;
            }

            if (_isShowing)
            {
                Logg.LogWarning("View is already showing");
                return;
            }

            _isShowing = true; 
            EventBeforeOpened?.Invoke();

            gameObject.SetActive(true); 
            _uiTransition.OpenTrans(() => 
            {
                if (_isShowing)
                {
                    EventAfterOpened?.Invoke();
                }
            });
        }

        public virtual void Hide()
        {
            if (_isShowing == false)
                return;

            _isShowing = false;
            EventBeforeClosed?.Invoke();
            _uiTransition.CloseTrans(() =>
            {
                gameObject.SetActive(false);
                _viewManager.RemovePopup(this);
                EventAfterClosed?.Invoke();
            });
        }

        public void CloseImmediately()
        {
            _isShowing = false;
            _uiTransition.AnyClose(() =>
            {
                gameObject.SetActive(false);
                _viewManager.RemovePopup(this);
            });
        }
    }
}