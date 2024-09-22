using System;
using CustomInspector;
using UnityEngine;
using UnityEngine.Events;

namespace OSK
{
    [RequireComponent(typeof(UITransition))]
    public class Popup : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool disableWhenNextPopupOpens;
        
        [Header("Events")] 
        [SerializeField] private bool showEvent;
        [ShowIf(nameof(showEvent)), SerializeField] private UnityEvent _afterInitialized;
        [ShowIf(nameof(showEvent)), SerializeField] private UnityEvent _beforeOpened;
        [ShowIf(nameof(showEvent)), SerializeField] private UnityEvent _afterOpened;
        [ShowIf(nameof(showEvent)), SerializeField] private UnityEvent _beforeClosed;
        [ShowIf(nameof(showEvent)), SerializeField] private UnityEvent _afterClosed;

        public bool DisableWhenNextPopupOpens 
        {
            get => disableWhenNextPopupOpens;
        }
      
        private UITransition _uiTransition;
        private PopupManager _popupManager;
        
        private bool _isShowing;
        public bool IsShowing => _isShowing;

        // private void OnValidate()
        // {
        //     name = GetType().Name;
        // }

        public virtual void Initialize(PopupManager popupManager)
        {
            _isShowing = false;
            _popupManager = popupManager;
            _uiTransition = GetComponent<UITransition>();
            _uiTransition.Initialize();
            _afterInitialized.Invoke();
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

        public virtual void Show()
        {
            _isShowing = true;
            _beforeOpened.Invoke();
            gameObject.SetActive(true);

            //_uiAudio.PlayOpeningSound(playAudio);
            _uiTransition.PlayOpeningTransition( () =>
            {
                _afterOpened.Invoke();
                //Debug.Log($"{gameObject.name} is opened");
            });
        }

        public virtual void Hide()
        {
            _isShowing = false;
            _beforeClosed.Invoke();
            //_uiAudio.PlayClosingSound(playAudio);
            _uiTransition.PlayClosingTransition( () =>
            {
                gameObject.SetActive(false);
                _popupManager.RemovePopup(this);
                _afterClosed.Invoke();
                //Debug.Log($"{gameObject.name} is closed");
            });
        }
    }
}