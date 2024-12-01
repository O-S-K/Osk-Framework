using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OSK
{
    public enum EViewType
    {
        None,
        Popup,
        Overlay,
        Screen
    }

    public class View : MonoBehaviour
    {
        [Header("View")] public EViewType viewType = EViewType.Popup;
        public int depth;
        public bool isAddToViewManager = true;
        public bool isPreloadSpawn = true;
        public bool isRemoveOnHide = false;
        [ReadOnly] public bool isInitOnScene = false;
        public bool IsShowing => _isShowing;
        [ShowInInspector, ReadOnly] private bool _isShowing;

        protected UITransition _uiTransition;
        protected ViewManager _viewManager;

        public bool isShowEvent = false;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventAfterInit;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventBeforeOpened;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventAfterOpened;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventBeforeClosed;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventAfterClosed;

        [Button]
        public void AddUITransition()
        {
            _uiTransition = gameObject.GetOrAdd<UITransition>();
        }

        public virtual void Initialize(ViewManager viewManager)
        {
            isInitOnScene = true;
            _viewManager = viewManager;

            if (GetComponent<UITransition>())
            {
                _uiTransition = GetComponent<UITransition>();
                _uiTransition.Initialize();
            }

            if (_viewManager == null)
            {
                Logg.LogError("View Manager is still null after initialization.");
            }


            SetOderInLayer(depth);
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
                switch (viewType)
                {
                    case EViewType.None:
                        canvas.sortingOrder = (0 + canvas.sortingOrder);
                        break;
                    case EViewType.Popup:
                        canvas.sortingOrder = (1000 + canvas.sortingOrder);
                        break;
                    case EViewType.Overlay:
                        canvas.sortingOrder = (10000 + canvas.sortingOrder);
                        break;
                    case EViewType.Screen:
                        canvas.sortingOrder = (-1000 + canvas.sortingOrder);
                        break;
                }
            }
        }

        public virtual void Open(object data = null)
        {
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

            if (_uiTransition == null)
            {
                EventAfterOpened?.Invoke();
            }
            else
            {
                _uiTransition.OpenTrans(() =>
                {
                    if (_isShowing)
                    {
                        EventAfterOpened?.Invoke();
                    }
                });
            }
        }

        public virtual void Hide()
        {
            if (_isShowing == false)
                return;

            _isShowing = false;
            EventBeforeClosed?.Invoke();

            if (_uiTransition == null)
            {
                gameObject.SetActive(false);
                EventAfterClosed?.Invoke();

                if (isRemoveOnHide)
                    _viewManager.Delete(this);
                else
                    _viewManager.RemovePopup(this);
            }
            else
            {
                _uiTransition.CloseTrans(() =>
                {
                    gameObject.SetActive(false);
                    EventAfterClosed?.Invoke();

                    if (isRemoveOnHide)
                        _viewManager.Delete(this);
                    else
                        _viewManager.RemovePopup(this);
                });
            }
        }

        public void CloseImmediately()
        {
            _isShowing = false;

            if (_uiTransition == null)
            {
                gameObject.SetActive(false);
                _viewManager.RemovePopup(this);
            }
            else
            {
                _uiTransition.AnyClose(() =>
                {
                    gameObject.SetActive(false);
                    _viewManager.RemovePopup(this);
                });
            }
        }

        public void Delete()
        {
            _viewManager.Delete(this);
        }
    }
}