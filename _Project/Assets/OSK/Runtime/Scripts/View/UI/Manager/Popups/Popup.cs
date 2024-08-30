using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OSK
{
    [RequireComponent(typeof(UITransition))]
    public class Popup : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private bool disableWhenNextPopupOpens;

        [Header("Events")] [SerializeField] private UnityEvent _afterInitialized;
        [SerializeField] private UnityEvent _beforeOpened;
        [SerializeField] private UnityEvent _afterOpened;
        [SerializeField] private UnityEvent _beforeClosed;
        [SerializeField] private UnityEvent _afterClosed;

        public bool DisableWhenNextPopupOpens => disableWhenNextPopupOpens;

        private UITransition _uiTransition;
        private PopupManager _popupManager;
        
        private bool _isShowing;
        public bool IsShowing => _isShowing;

        public virtual void Initialize(PopupManager popupManager)
        {
            _isShowing = false;
            _popupManager = popupManager;
            _uiTransition = GetComponent<UITransition>();
            _uiTransition.Initialize();
            _afterInitialized.Invoke();
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