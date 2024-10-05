using System;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class PopupManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] [SerializeField] private List<Popup> ListPopups;
        [ShowInInspector, ReadOnly] private Stack<Popup> _popupHistory = new Stack<Popup>();
        [SerializeField] private ListUIPopupSO _listUIPopupSo;
        public ListUIPopupSO ListUIPopupSo => _listUIPopupSo;
        
        
        
        public void Initialize()
        {
            PreloadPopups();
        }

        private void PreloadPopups()
        {
            ListPopups.Clear();
            var listUIPopupSo = _listUIPopupSo.Popups;
            if (listUIPopupSo == null)
            {
                OSK.Logg.LogError("[Popup] ListUIPopupSO.Popups is null");
                return;
            }
            
            for (int i = 0; i < listUIPopupSo.Count; i++)
            {
                var popup = Instantiate(listUIPopupSo[i], transform);
                popup.Initialize(this);
                popup.transform.localPosition = Vector3.zero;
                popup.transform.localScale = Vector3.one;
                ListPopups.Add(popup);
            } 
        }
        
        public T Create<T>(string path, bool isHidePrevPopup) where T : Popup
        {
            if (IsExist<T>())
            {
                return Show<T>(isHidePrevPopup);
            }
            else
            {
                var popup = SpawnFromResource<T>(path);
                popup.Show();
                if(!ListPopups.Contains(popup)) 
                    ListPopups.Add(popup);
                return popup;
            }
        }
        
        public T ShowOverlay<T>(string path) where T : Popup
        {
            var popup = SpawnFromResource<T>(path);
            popup.Show();
            popup.transform.SetTopSibling();
            return popup;
        }
 
        public void Delete<T>(T popup) where T : Popup
        {
            ListPopups.Remove(popup);
            Destroy(popup.gameObject);
        }
        
        
        public T Show<T>(bool isHidePrevPopup) where T : Popup
        {
            foreach (var popup in ListPopups)
            {
                if (popup is T)
                {
                    if (popup.IsShowing && popup.gameObject.activeInHierarchy)
                    {
                        _popupHistory.Push(popup);
                        OSK.Logg.Log("Popup is already showing");
                        break;
                    }
                    Show(popup, isHidePrevPopup);
                    return (T)popup;
                }
            }
            return null;
        }
        
        public List<Popup> GetAllPopups()
        {
            return ListPopups;
        }
        
        public T ShowAny<T>() where T : Popup
        {
            foreach (var popup in ListPopups)
            {
                if (popup is T)
                {
                    popup.Show();
                    return (T)popup;
                }
            }
            return null;
        }
        
        
        public Popup Show(Popup popup, bool isHidePrevPopup)
        {
            if (isHidePrevPopup && _popupHistory.Count > 0)
            {
                var prevPopup = _popupHistory.Peek();
                prevPopup.Hide();
            }

            _popupHistory.Push(popup);
            popup.Show();
            return popup;
        }

        public T Get<T>() where T : Popup
        {
            foreach (var popup in ListPopups)
            {
                if (popup is T)
                    return (T)popup;
            }
            return null;
        }
        
        public Popup Get(Popup popup)
        {
            foreach (var p in ListPopups)
            {
                if (p == popup)
                    return p;
            }
            return null;
        }
        

        public void Remove(bool isHidePrevPopup = false)
        {  
            if (_popupHistory.Count <= 0) 
                return;

            var currentPopup = _popupHistory.Pop();
            currentPopup.Hide();

            if (_popupHistory.Count > 0)
            {
                var prevPopup = _popupHistory.Peek();
                if (isHidePrevPopup)
                    prevPopup.Show();
            }
        }
        
        public void Hide(Popup popup)
        {
            if (_popupHistory.Count <= 0) return;
            if (_popupHistory.Peek() == popup)
            {
                Remove();
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
            Show(popup, true);
        }
        
               
        private T SpawnFromResource<T>(string path) where T : Popup
        {
            var popup = Instantiate(Resources.Load<T>(path), transform);
            
            if(popup == null)
                throw new Exception($"[Popup] Can't find popup with path: {path}");
            popup.Initialize(this);
            return popup;
        }
        private bool IsExist<T>() where T : Popup
        {
            foreach (var popup in ListPopups)
            {
                if (popup is T)
                    return true;
            }

            return false;
        }

    }
}
