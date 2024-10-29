using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class DataViewUI
    {
        public string index;
        public View view;
    }
    
    [CreateAssetMenu(fileName = "ListViewSO", menuName = "OSK/UI/ListViewSO")]
    public class ListViewSO : ScriptableID
    { 
        [TableList] [SerializeField]
        private List<DataViewUI> _listView = new List<DataViewUI>();
        public List<DataViewUI> Views => _listView;

#if UNITY_EDITOR
        private void OnValidate()
        {
            // check unique ui screen  type
            var views = new List<DataViewUI>();
            foreach (var view in _listView)
            {
                if (views.Contains(view))
                {
                    OSK.Logg.LogError($"Popup Type {view} exists in the list. Please remove it.");
                }
                else
                {
                    views.Add(view);
                }
            }
        }

        [Button("Add All View Form Resources")]
        public void AddAllViewFormResources()
        {
            _listView.Clear();
            var listViews = Resources.LoadAll<View>("").ToList();

            for (int i = 0; i < listViews.Count; i++)
            {
                var popup = listViews[i];
                var data = new DataViewUI();
                data.view = popup;
                data.index = data.view.index.ToString();
                _listView.Add(data);
            }
            
            // sort by index
            _listView = _listView.OrderBy(x => int.Parse(x.index)).ToList();
        }
#endif
    }
}