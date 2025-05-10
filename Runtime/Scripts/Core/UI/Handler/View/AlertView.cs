using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    [System.Serializable]
    public class AlertSetup
    {
        public string title = "";
        public string message = ""; 
        public Action onOk = null;
        public Action onCancel = null;
        public float timeHide = 0;
    }
    
    public class AlertView : OSK.View
    {
        /// Title of the alert.
        public GameObject title;
        /// Message of the alert.
        public GameObject message;
        /// Buttons of the alert.
        public Button okButton;
        /// Cancel button of the alert.
        public Button cancelButton;
        /// Time to hide the alert.
        public float timeHide = 0;

        public void SetData(AlertSetup setup)
        {
            if(setup == null)
            {
                Logg.LogError("AlertView: setup is null.");
                return;
            }
            SetTile(setup.title);
            SetMessage(setup.message);
            SetOkButton(setup.onOk);
            SetCancelButton(setup.onCancel);
            SetTimeHide(setup.timeHide);
        } 

        private void SetTile(string _title)
        {
            SetTextComponent(title, _title, "Title");
        }
        
        private void SetMessage(string _message)
        {
            SetTextComponent(message, _message, "Message");
        }
        
        private void SetTextComponent(GameObject target, string text, string errorContext)
        {
            if (string.IsNullOrEmpty(text) || target == null)
                return;

            if (target.TryGetComponent<TMP_Text>(out var tmp))
            {
                tmp.text = text;
            }
            else if (target.TryGetComponent<Text>(out var legacyText))
            {
                legacyText.text = text;
            }
            else
            {
                Logg.LogError($"[AlertView] {errorContext}: No Text or TMP_Text component found.");
            }
        }

        private void SetOkButton(Action onOk)
        {
            if (onOk == null)
            {
                okButton?.gameObject.SetActive(false);
                return;
            }

            okButton.BindButton(() =>
            {
                onOk?.Invoke();
                OnClose();
            });
        }

        private void SetCancelButton(Action onCancel)
        {
            if (onCancel == null)
            {
                cancelButton?.gameObject.SetActive(false);
                return;
            }

            cancelButton.BindButton(() =>
            {
                onCancel?.Invoke();
                OnClose();
            });
        }

        public virtual void SetTimeHide(float time)
        {
            if (time <= 0)
                return;
            timeHide = time;
            DOVirtual.DelayedCall(time, OnClose);
        }

        protected virtual void OnDisable()
        {
            okButton?.onClick.RemoveAllListeners();
            cancelButton?.onClick.RemoveAllListeners();
        }
        
        protected virtual void OnDestroy()
        {
            okButton?.onClick.RemoveAllListeners();
            cancelButton?.onClick.RemoveAllListeners();
        }

        public virtual void OnClose()
        {
            Logg.Log("AlertView: OnClose called. Time hide left " + timeHide);
            Destroy(gameObject);
        }
    }
}