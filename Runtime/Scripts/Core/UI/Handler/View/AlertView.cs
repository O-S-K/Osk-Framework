using System;
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
        public TMPro.TMP_Text title;
        public TMPro.TMP_Text message;
        public Button okButton;
        public Button cancelButton;

        public void SetData(AlertSetup setup)
        {
            SetTile(setup.title);
            SetMessage(setup.message);
            SetOkButton(setup.onOk);
            SetCancelButton(setup.onCancel);
            SetTimeHide(setup.timeHide);
        } 

        private void SetTile(string title)
        {
            if (string.IsNullOrEmpty(title))
                return;
            this.title.text = title;
        }

        private void SetMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            this.message.text = message;
        }

        private void SetOkButton(Action onOk)
        {
            if (onOk == null)
            {
                if (okButton != null)
                {
                    okButton.gameObject.SetActive(false);
                }

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
                if (cancelButton != null)
                {
                    cancelButton.gameObject.SetActive(false);
                }

                return;
            }

            cancelButton.BindButton(() =>
            {
                onCancel?.Invoke();
                OnClose();
            });
        }

        public void SetTimeHide(float time)
        {
            if (time <= 0)
                return;
            this.DoDelay(time, OnClose);
        }

        private void OnDisable()
        {
            okButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }


        public virtual void OnClose()
        {
            Destroy(gameObject);
        }
    }
}