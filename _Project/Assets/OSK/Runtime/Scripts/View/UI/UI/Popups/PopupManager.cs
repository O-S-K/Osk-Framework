using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private Popup[] popups;
        private Stack<Popup> _popupHistory = new();

        public void Initialize()
        {
            foreach (var popup in popups)
            {
                popup.Initialize();
                popup.Close();
            } 
        }

        private void Show(Popup popup)
        {
            if (popup == null) return;
            if (_popupHistory.Count > 0 && _popupHistory.Peek() == popup) return;

            if (_popupHistory.Count > 0)
            {
                var prevPopup = _popupHistory.Peek();
                if (prevPopup.DisableWhenNextPopupOpens)
                    prevPopup.Close();
            }

            _popupHistory.Push(popup);
            popup.Open(true);
        }
        
        public T Show<T>() where T : Popup
        {
            var popup = GetPopup<T>();
            Show(popup);
            return popup;
        }
        
        public T GetPopup<T>() where T : Popup
        {
            foreach (var popup in popups)
            {
                if (popup is T)
                    return (T)popup;
            }

            return null;
        }
        

        public void RemovePopup()
        {
            if (_popupHistory.Count <= 0) return;

            var currentPopup = _popupHistory.Pop();
            currentPopup.Close(true);

            if (_popupHistory.Count > 0)
            {
                var prevPopup = _popupHistory.Peek();
                if (prevPopup.DisableWhenNextPopupOpens)
                    prevPopup.Open();
            }
        }

        public void RemoveAllPopups()
        {
            while (_popupHistory.Count > 0)
            {
                var currentPage = _popupHistory.Pop();
                currentPage.Close();
            }
        }

        public void RemoveAllAndAddPopup(Popup popup)
        {
            RemoveAllPopups();
            Show(popup);
        }
    }
}
