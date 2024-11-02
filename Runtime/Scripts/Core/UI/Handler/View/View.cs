using System;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        public bool isOverlay = false;

        [Header("Events")] [SerializeField] protected bool showEvent;

        [ShowIf(nameof(showEvent))] public Action EventAfterInit;
        [ShowIf(nameof(showEvent))] public Action EventBeforeOpened;
        [ShowIf(nameof(showEvent))] public Action EventAfterOpened;
        [ShowIf(nameof(showEvent))] public Action EventBeforeClosed;
        [ShowIf(nameof(showEvent))] public Action EventAfterClosed;

        protected UITransition _uiTransition;
        protected ViewManager ViewManager;

        public bool IsShowing => _isShowing;
        [ShowInInspector, ReadOnly] private bool _isShowing;


        public virtual void Initialize(ViewManager viewManager)
        {
            gameObject.SetActive(false);
            if (isOverlay)
                transform.SetTopSibling();
            _isShowing = false;
            ViewManager = viewManager;
            _uiTransition = GetComponent<UITransition>();
            _uiTransition.Initialize();
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
            _isShowing = true;
            EventBeforeOpened?.Invoke();

            gameObject.SetActive(false);
            gameObject.SetActive(true);

            _uiTransition.OpenTrans(() => { EventAfterOpened?.Invoke(); });
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
                ViewManager.RemovePopup(this);
                EventAfterClosed?.Invoke();
            });
        }

        public void CloseImmediately()
        {
            _isShowing = false;
            _uiTransition.AnyClose(() =>
            {
                gameObject.SetActive(false);
                ViewManager.RemovePopup(this);
            });
        }
    }
}