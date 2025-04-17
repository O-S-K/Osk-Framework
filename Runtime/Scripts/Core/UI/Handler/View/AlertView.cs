using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
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
        public GameObject title;
        public GameObject message;
        public Button okButton;
        public Button cancelButton;
        public float timeHide;

        public void SetData(AlertSetup setup)
        {
            SetTile(setup.title);
            SetMessage(setup.message);
            SetOkButton(setup.onOk);
            SetCancelButton(setup.onCancel);
            SetTimeHide(setup.timeHide);
        } 

        private void SetTile(string _title)
        {
            if (string.IsNullOrEmpty(_title))
                return;
            if (title.GetComponent<TMP_Text>())
            {
                title.GetComponent<TMP_Text>().text = _title;
            }
            else if (title.GetComponent<Text>())
            {
                title.GetComponent<Text>().text = _title;
            }
            else
            {
                Logg.LogError("AlertView: title No Text or TMP_Text component found on message object.");
            }
        }
        

        private void SetMessage(string _message)
        {
            if (string.IsNullOrEmpty(_message))
                return;
            if (message.GetComponent<TMP_Text>())
            {
                message.GetComponent<TMP_Text>().text = _message;
            }
            else if (message.GetComponent<Text>())
            {
                message.GetComponent<Text>().text = _message;
            }
            else
            {
                Logg.LogError("AlertView: Message No Text or TMP_Text component found on message object.");
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
            this.DoDelay(time, OnClose);
        }

        protected virtual void OnDisable()
        {
            okButton?.onClick.RemoveAllListeners();
            cancelButton?.onClick.RemoveAllListeners();
        }

        public virtual void OnClose()
        {
            Destroy(gameObject);
        }
    }
}