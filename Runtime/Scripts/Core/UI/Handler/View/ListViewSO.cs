using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    [System.Serializable]
    public class DataViewUI
    {
        public string depth;
        public View view;
    }

    [CreateAssetMenu(fileName = "ListViewSO", menuName = "OSK/UI/ListViewSO")]
    public class ListViewSO : ScriptableID
    {
        [TableList, SerializeField] private List<DataViewUI> _listView = new List<DataViewUI>();
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
            var listViews = Resources.LoadAll<View>("").ToList().FindAll(x => x.isAddToViewManager);

            foreach (var popup in listViews)
            {
                // Kiểm tra xem view đã tồn tại trong danh sách chưa
                if (_listView.Any(x => x.view == popup))
                {
                    // Nếu đã tồn tại, bỏ qua việc thêm vào danh sách
                    continue;
                }

                var data = new DataViewUI();
                data.view = popup;

                switch (data.view.viewType)
                {
                    case EViewType.None:
                        data.depth = (0 + data.view.depth).ToString();
                        break;
                    case EViewType.Popup:
                        data.depth = (100 + data.view.depth).ToString();
                        break;
                    case EViewType.Overlay:
                        data.depth = (1000 + data.view.depth).ToString();
                        break;
                    case EViewType.Screen:
                        data.depth = (-100 + data.view.depth).ToString();
                        break;
                }

                _listView.Add(data);
            }

            // Sắp xếp theo depth
            // _listView = _listView.OrderBy(x => int.Parse(x.depth)).ToList();
        }
#endif
    }
}