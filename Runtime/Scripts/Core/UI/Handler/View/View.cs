using System;
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
        [Header("Settings")]
        [EnumToggleButtons]
        public EViewType viewType = EViewType.Popup;
        public int depth;
        
        [Space]
        [ToggleLeft] public bool isAddToViewManager = true;
        [ToggleLeft] public bool isPreloadSpawn = true;
        [ToggleLeft] public bool isRemoveOnHide = false;
        
        [ReadOnly] 
        [ToggleLeft] public bool isInitOnScene;
        public bool IsShowing => _isShowing;
        [ShowInInspector, ReadOnly] 
        [ToggleLeft] private bool _isShowing;
        private UITransition _uiTransition;
        private ViewContainer _viewContainer;

        [Space]
        [ToggleLeft] public bool isShowEvent = false;
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

        public virtual void Initialize(ViewContainer viewContainer)
        {
            if (isInitOnScene) return;

            isInitOnScene = true;
            _viewContainer = viewContainer; 

            _uiTransition = GetComponent<UITransition>();
            _uiTransition?.Initialize();

            if (_viewContainer == null)
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
                canvas.sortingOrder = viewType switch
                {
                    EViewType.None => (0 + canvas.sortingOrder),
                    EViewType.Popup => (1000 + canvas.sortingOrder),
                    EViewType.Overlay => (10000 + canvas.sortingOrder),
                    EViewType.Screen => (-1000 + canvas.sortingOrder),
                    _ => canvas.sortingOrder
                };
            }
            else
            {
                var childPages = _viewContainer.GetSortedChildPages(_viewContainer.transform);
                if (childPages.Count == 0)
                    return;

                var insertIndex = _viewContainer.FindInsertIndex(childPages, order);
                if (insertIndex == childPages.Count) transform.SetAsLastSibling();
                else transform.SetSiblingIndex(insertIndex);
            }
        }


        public virtual void Open(object data = null)
        {
            if (!IsViewContainerInitialized() || IsAlreadyShowing()) return;

            _isShowing = true;
            EventBeforeOpened?.Invoke();
            gameObject.SetActive(true);

            if (_uiTransition != null) _uiTransition.OpenTrans(() => EventAfterOpened?.Invoke());
            else EventAfterOpened?.Invoke();
        }

        public virtual void Hide()
        {
            if (!_isShowing) return;

            _isShowing = false;
            EventBeforeClosed?.Invoke();

            if (_uiTransition != null) _uiTransition.CloseTrans(FinalizeHide);
            else FinalizeHide();
        }

        public void CloseImmediately()
        {
            _isShowing = false;

            if (_uiTransition != null) _uiTransition.AnyClose(FinalizeImmediateClose);
            else FinalizeImmediateClose();
        }

        private bool IsViewContainerInitialized()
        {
            if (_viewContainer == null)
            {
                Logg.LogError("View Manager is null. Ensure that the View has been initialized before calling Open.");
                return false;
            }

            return true;
        }

        private bool IsAlreadyShowing()
        {
            if (_isShowing)
            {
                Logg.LogWarning("View is already showing");
                return true;
            }

            return false;
        }

        private void FinalizeHide()
        {
            gameObject.SetActive(false);
            EventAfterClosed?.Invoke();

            if (isRemoveOnHide) _viewContainer.Delete(this);
            else _viewContainer.RemovePopup(this);
        }

        private void FinalizeImmediateClose()
        {
            gameObject.SetActive(false);
            _viewContainer.RemovePopup(this);
        }

        public void Delete()
        {
            _viewContainer.Delete(this);
        }
    }
}