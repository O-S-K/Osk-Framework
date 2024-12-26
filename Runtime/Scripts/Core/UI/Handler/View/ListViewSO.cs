using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class DataViewUI
    {
#if UNITY_EDITOR
        public string path;
#endif
        public string depth;
        public View view;
    }

    [CreateAssetMenu(fileName = "ListViewSO", menuName = "OSK/UI/ListViewSO")]
    public class ListViewSO : ScriptableObject
    {
        [TableList, SerializeField] private List<DataViewUI> _listView = new List<DataViewUI>();
        public List<DataViewUI> Views => _listView; 
    }
}