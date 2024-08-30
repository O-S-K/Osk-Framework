using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private List<Popup> popups;
        [ShowInInspector, ReadOnly] private Stack<Popup> _popupHistory = new();

        public void Initialize()
        {
            foreach (var popup in popups)
            {
                popup.Initialize(this);
                popup.Hide();
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
                    prevPopup.Hide();
            }

            _popupHistory.Push(popup);
            popup.Show();
        }
        
        public T Create<T>(string path) where T : Popup
        {
            if (IsExist<T>())
            {
                return Show<T>();
            }
            else
            {
                var popup = Instantiate(Resources.Load<T>(path), transform);
                popup.Initialize(this);
                popup.Show();
                if(!popups.Contains(popup)) 
                    popups.Add(popup);
                return popup;
            }
        }
        
        private bool IsExist<T>() where T : Popup
        {
            foreach (var popup in popups)
            {
                if (popup is T)
                    return true;
            }

            return false;
        }
        
        public void Delete<T>(T popup) where T : Popup
        {
            popups.Remove(popup);
            Destroy(popup.gameObject);
        }
        
        public T Show<T>() where T : Popup
        {
            foreach (var popup in popups)
            {
                if (popup is T)
                {
                    if (popup.IsShowing)
                    {
                        _popupHistory.Push(popup);
                        Debug.Log("Popup is already showing");
                        break;
                    }
                    Show(popup);
                    return (T)popup;
                }
            }
            return null;
        }
        
        public T Get<T>() where T : Popup
        {
            foreach (var popup in popups)
            {
                if (popup is T)
                    return (T)popup;
            }

            return null;
        }
        

        public void Remove()
        {
            if (_popupHistory.Count <= 0) return;

            var currentPopup = _popupHistory.Pop();
            currentPopup.Hide();

            if (_popupHistory.Count > 0)
            {
                var prevPopup = _popupHistory.Peek();
                if (prevPopup.DisableWhenNextPopupOpens)
                    prevPopup.Show();
            }
        }
        
        public void RemovePopup(Popup popup)
        {
            if (_popupHistory.Count <= 0) return;

            if (_popupHistory.Peek() == popup)
            {
                Remove();
            }
        }

        public void RemoveAll()
        {
            while (_popupHistory.Count > 0)
            {
                var currentPage = _popupHistory.Pop();
                currentPage.Hide();
            }
        }

        public void RemoveAllAndAdd(Popup popup)
        {
            RemoveAll();
            Show(popup);
        }
    }
}
