using UnityEngine;
using CustomInspector;
using System.Collections.Generic;
using System.Linq;

namespace OSK
{
    public class ViewManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] [SerializeField]
        private List<View> _listView = new List<View>();
        private Stack<View> _viewHistory = new Stack<View>();
        public Stack<View> ListViewHistory => _viewHistory;


        #region Init

        public void Initialize()
        {
            Preload();
        }

        private void Preload()
        {
            _listView.Clear();
            var listUIPopupSo = Main.Configs.data.listViewS0.Views;
            if (listUIPopupSo == null)
            {
                Logg.LogError("[View] is null");
                return;
            }

            for (int i = 0; i < listUIPopupSo.Count; i++)
            {
                var popup = Instantiate(listUIPopupSo[i].view, transform);
                popup.Initialize(this);
                popup.transform.localPosition = Vector3.zero;
                popup.transform.localScale = Vector3.one;
                _listView.Add(popup);
            }
        }

        #endregion

        #region Spawn

        public T Spawn<T>(string path, bool isCache, bool hidePrevView) where T : View
        {
            if (IsExist<T>())
            {
                return Open<T>(hidePrevView);
            }
            else
            {
                var _view = SpawnFromResource<T>(path);
                if (isCache)
                {
                    if (!_listView.Contains(_view))
                        _listView.Add(_view);
                }

                return _view;
            }
        }

        #endregion

        #region Open

        public T Open<T>(bool hidePrevView) where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T)
                {
                    if (_view.IsShowing && _view.gameObject.activeInHierarchy)
                    {
                        _viewHistory.Push(_view);
                        OSK.Logg.Log($"[View] {_view.name} is already showing");
                        break;
                    }

                    Open(_view, hidePrevView);
                    return (T)_view;
                }
            }

            return null;
        }

        public T TryOpen<T>(bool hidePrevView) where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T)
                { 
                    if (_viewHistory.Count > 0 && hidePrevView)
                    {
                        var _prevView = _viewHistory.Peek();
                        _prevView.CloseImmediately(); 
                        Logg.Log($"[View] Close previous view: {_prevView.name}");
                    }
                    _view.Open();
                    return (T)_view;
                }
            }

            return null;
        }

        public void OpenPrevious()
        {
            if (_viewHistory.Count > 0)
            {
                var _prevView = _viewHistory.Pop();
                _prevView.Open();
                Logg.Log($"[View] Open previous view: {_prevView.name}");
            }
        }

        public View Open(View view, bool hidePrevView)
        {
            if (hidePrevView && _viewHistory.Count > 0)
            {
                var prevView = _viewHistory.Peek();
                prevView.Hide();
                Logg.Log($"[View] Hide previous view: {prevView.name}");
            }

            _viewHistory.Push(view);
            view.Open();
            return view;
        }

        #endregion

        #region Get

        public T Get<T>() where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T)
                {
                    return (T)_view;
                }
            }

            return null;
        }

        public T GetIsActive<T>() where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T && _view.IsShowing)
                {
                    return (T)_view;
                }
            }

            return null;
        }

        public View Get(View view)
        {
            foreach (var _view in _listView)
            {
                if (_view == view)
                    return _view;
            }

            return null;
        }

        public List<View> GetAll()
        {
            return _listView;
        }

        #endregion

        #region Hide

        public void Hide(View view)
        {
            if (_viewHistory.Count <= 0) return;
            if (_viewHistory.Peek() == view)
            {
                Remove();
            }
        }

        public void HideIgnore<T>() where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T)
                    continue;
                if (_view.IsShowing)
                {
                    _view.Hide();
                }
            }
        }

        public void HideIgnore<T>(T[] viewsToKeep) where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T && !viewsToKeep.Contains(_view as T))
                {
                    if (_view.IsShowing)
                    {
                        _view.Hide();
                    }
                }
            }
        }

        public void HideAll()
        {
            foreach (var _view in _listView)
            {
                if (_view.IsShowing)
                {
                    _view.Hide();
                }
            }
        }

        #endregion

        #region Remove

        public void Remove(bool hidePrevView = false)
        {
            if (_viewHistory.Count <= 0)
                return;

            var _curView = _viewHistory.Pop();
            _curView.Hide();

            if (hidePrevView)
                OpenPrevious();
        }

        public void RemovePopup(View view)
        {
            if (_viewHistory.Count <= 0) return;
            if (_viewHistory.Peek() == view)
            {
                Remove();
            }
        }

        public void RemoveAll()
        {
            while (_viewHistory.Count > 0)
            {
                var _curView = _viewHistory.Pop();
                _curView.Hide();
            }
        }


        public void RemoveAllAndAdd(View view)
        {
            RemoveAll();
            Open(view, true);
        }

        #endregion

        #region Delete

        public void Delete<T>(T view) where T : View
        {
            _listView.Remove(view);
            Destroy(view.gameObject);
        }

        #endregion

        #region Private

        private T SpawnFromResource<T>(string path) where T : View
        {
            var _view = Instantiate(Resources.Load<T>(path), transform);

            if (_view == null)
            {
                Logg.LogError($"[Popup] Can't find popup with path: {path}");
                return null;
            }

            _view.Initialize(this);
            return _view;
        }

        private bool IsExist<T>() where T : View
        {
            foreach (var _view in _listView)
            {
                if (_view is T)
                    return true;
            }

            return false;
        }

        #endregion
    }
}