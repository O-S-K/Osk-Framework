using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace OSK
{
    public class ViewContainer : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] [SerializeField]
        private List<View> _listViewInit = new List<View>();

        [ShowInInspector, ReadOnly] [SerializeField]
        private List<View> _listCacheView = new List<View>();

        private Stack<View> _viewHistory = new Stack<View>();
        public Stack<View> ListViewHistory => _viewHistory;


        #region Init
         

        public void Initialize()
        {
            if (Main.ConfigsManager.init == null || Main.ConfigsManager.init.data == null || Main.ConfigsManager.init.data.listViewS0 == null)
                return;
            Preload();
        }

        private void Preload()
        {
            var listUIPopupSo = Main.ConfigsManager.init.data.listViewS0.Views;
            if (listUIPopupSo == null)
            {
                Logg.LogError("[View] is null");
                return;
            }

            _listViewInit.Clear();
            _listViewInit = listUIPopupSo.Select(view => view.view).ToList();

            for (int i = 0; i < _listViewInit.Count; i++)
            {
                if (_listViewInit[i].isPreloadSpawn)
                {
                    SpawnViewCache(_listViewInit[i]);
                }
            }
        }

        #endregion

        #region Spawn

        public T Spawn<T>(T view, bool hidePrevView) where T : View
        {
            if (IsExist<T>())
                return Open<T>(hidePrevView);
            else
                return SpawnViewCache(view);
        }

        public T Spawn<T>(string path, bool isCache, bool hidePrevView) where T : View
        {
            if (IsExist<T>())
            {
                return Open<T>(hidePrevView);
            }
            else
            {
                var view = SpawnFromResource<T>(path);
                if (isCache)
                {
                    if (_listCacheView.Contains(view))
                        _listCacheView.Add(view);
                }

                return view;
            }
        }

        public T SpawnViewCache<T>(T _view) where T : View
        {
            var view = Instantiate(_view, transform);
            view.gameObject.SetActive(false);
            view.Initialize(this);

            view.transform.localPosition = Vector3.zero;
            view.transform.localScale = Vector3.one;

            Logg.Log($"[View] Spawn view: {view.name}");
            if (!_listCacheView.Contains(view))
                _listCacheView.Add(view);
            return view;
        }

        #endregion

        #region Open

        public View Open(View _view, bool hidePrevView, bool checkShowing = true)
        {
            var view = _listCacheView.FirstOrDefault(v => v == _view);
            if (hidePrevView && _viewHistory.Count > 0)
            {
                var prevView = _viewHistory.Peek();
                prevView.Hide();
            }

            if (view == null)
            {
                var viewPrefab = _listViewInit.FirstOrDefault(v => v == _view);
                if (viewPrefab == null)
                {
                    Logg.LogError($"[View] Can't find view prefab for type: {_view.GetType().Name}");
                    return null;
                }

                view = SpawnViewCache(viewPrefab);
            }

            if (view.IsShowing && checkShowing)
            {
                return view;
            }

            view.Open();
            _viewHistory.Push(view);
            Logg.Log($"[View] Opened view: {view.name}");
            return view;
        }

        public T Open<T>(bool hidePrevView, bool checkShowing = true) where T : View
        {
            var view = _listCacheView.FirstOrDefault(v => v is T) as T;
            if (hidePrevView && _viewHistory.Count > 0)
            {
                var prevView = _viewHistory.Peek();
                prevView.Hide();
            }

            if (view == null)
            {
                var viewPrefab = _listViewInit.FirstOrDefault(v => v is T) as T;
                if (viewPrefab == null)
                {
                    Logg.LogError($"[View] Can't find view prefab for type: {typeof(T).Name}");
                    return null;
                }

                view = SpawnViewCache(viewPrefab);
            }

            if (view.IsShowing && checkShowing)
            {
                return view;
            }

            view.Open();
            _viewHistory.Push(view);
            Logg.Log($"[View] Opened view: {view.name}");
            return view;
        }

        public T TryOpen<T>(bool hidePrevView) where T : View
        {
            return Open<T>(hidePrevView, false);
        }

        public void OpenPrevious()
        {
            if (_viewHistory.Count > 0)
            {
                var prevView = _viewHistory.Pop();
                prevView.Open();
                Logg.Log($"[View] Open previous view: {prevView.name}");
            }
        }

        #endregion

        #region Get

        public View Get(View _view, bool isInitOnScene = true)
        {
            var view = GetAll(isInitOnScene).Find(x => x == _view);
            if (view == null)
            {
                Logg.LogError($"[View] Can't find view: {_view.name}");
                return null;
            }

            if (!view.isInitOnScene)
            {
                Logg.LogError($"[View] {_view.name} is not init on scene");
            }

            return view;
        }

        public T Get<T>(bool isInitOnScene = true) where T : View
        {
            var view = GetAll(isInitOnScene).Find(x => x is T) as T;
            if (view == null)
            {
                Logg.LogError($"[View] Can't find view: {typeof(T).Name}");
                return null;
            }

            if (!view.isInitOnScene)
            {
                Logg.LogError($"[View] {typeof(T).Name} is not init on scene");
            }

            return view;
        }

        public View Get(View _view)
        {
            var view = GetAll(true).Find(x => x == _view);
            if (view == null)
            {
                Logg.LogError($"[View] Can't find view: {_view.name}");
                return null;
            }

            return view;
        }

        public List<View> GetAll(bool isInitOnScene)
        {
            if (isInitOnScene)
                return _listCacheView;

            var views = _listViewInit.FindAll(x => x.isInitOnScene);
            if (views.Count <= 0)
            {
                Logg.LogError($"[View] Can't find any view");
                return null;
            }

            return views;
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
            foreach (var view in _listCacheView)
            {
                if (view is T)
                    continue;
                if (view.IsShowing)
                {
                    view.Hide();
                }
            }
        }

        public void HideIgnore<T>(T[] viewsToKeep) where T : View
        {
            foreach (var view in _listCacheView)
            {
                if (view is T && !viewsToKeep.Contains(view as T))
                {
                    if (view.IsShowing)
                    {
                        view.Hide();
                    }
                }
            }
        }

        public void HideAll()
        {
            foreach (var view in _listCacheView.Where(view => view.IsShowing))
            {
                view.Hide();
            }
        }

        #endregion

        #region Remove

        public void RemovePopup(View view)
        {
            if (_viewHistory.Count <= 0) return;
            if (_viewHistory.Peek() == view)
            {
                Remove();
            }
        }

        public void Remove(bool hidePrevView = false)
        {
            if (_viewHistory.Count <= 0)
                return;

            var curView = _viewHistory.Pop();
            curView.Hide();

            if (hidePrevView)
                OpenPrevious();
        }

        public void RemoveAll()
        {
            while (_viewHistory.Count > 0)
            {
                var curView = _viewHistory.Pop();
                curView.Hide();
            }
        }

        #endregion

        #region Delete

        public void Delete<T>(T view, Action action = null) where T : View
        {
            if (!_listCacheView.Contains(view))
                return;

            Logg.Log($"[View] Delete view: {view.name}");
            _listCacheView.Remove(view);
            Destroy(view.gameObject);
            action?.Invoke();
        }

        #endregion
        
        public List<View> GetSortedChildPages(Transform container)
        {
            List<View> childPages = new List<View>();
            for (int i = 0; i < container.childCount; i++)
            {
                var childPage = container.GetChild(i).GetComponent<View>();
                if (childPage != null)
                    childPages.Add(childPage);
            }
            return childPages;
        }
        
        public int FindInsertIndex(List<View> childPages, int depth)
        {
            int left = 0, right = childPages.Count - 1;
            int insertIndex = childPages.Count;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (depth < childPages[mid].depth)
                {
                    insertIndex = mid; 
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return insertIndex;
        }

        #region Private

        private T SpawnFromResource<T>(string path) where T : View
        {
            var view = Instantiate(Resources.Load<T>(path), transform);

            if (view == null)
            {
                Logg.LogError($"[Popup] Can't find popup with path: {path}");
                return null;
            }

            return SpawnViewCache(view);
        }

        private bool IsExist<T>() where T : View
        {
            return _listCacheView.Exists(x => x is T);
        }

        #endregion
        
        
    }
}