using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace OSK
{
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor : Editor
    {
        private DataManager _dataManager;
        private Dictionary<System.Type, List<IData>> _dataStore;
        private List<System.Type> _dataTypes;
        private int _selectedTypeIndex = 0;

        private void OnEnable()
        {
            _dataManager = (DataManager)target;

            // Lấy danh sách type của tất cả IData đang có
            var dataField = typeof(DataManager).GetField("k_DataStore", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dataField != null)
            {
                _dataStore = dataField.GetValue(_dataManager) as Dictionary<System.Type, List<IData>>;
                _dataTypes = _dataStore?.Keys.ToList() ?? new List<System.Type>();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("📌 List Datas", EditorStyles.boldLabel);

            if (_dataTypes.Count > 0)
            {
                // Tạo danh sách dropdown với tùy chọn "All Data" ở đầu
                string[] typeNames = new string[] { "All Data" }.Concat(_dataTypes.Select(t => t.Name)).ToArray();
                _selectedTypeIndex = EditorGUILayout.Popup("Select Data Type", _selectedTypeIndex, typeNames);

                EditorGUILayout.Space();

                if (_selectedTypeIndex == 0) // Nếu chọn "All Data"
                {
                    foreach (var dataType in _dataTypes)
                    {
                        DisplayData(dataType);
                    }
                }
                else // Chọn một loại dữ liệu cụ thể
                {
                    DisplayData(_dataTypes[_selectedTypeIndex - 1]);
                }
            }
            else
            {
                EditorGUILayout.LabelField("⚠️ No registered data.", EditorStyles.helpBox);
            }
        }

        private void DisplayData(System.Type selectedType)
        {
            if (_dataStore.TryGetValue(selectedType, out var dataList) && dataList.Count > 0)
            {
                EditorGUILayout.LabelField($"🔹 {selectedType.Name} ({dataList.Count} items)", EditorStyles.boldLabel);
                
                foreach (var item in dataList)
                {
                    DrawIData(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField($"⚠️ No data found for {selectedType.Name}.", EditorStyles.helpBox);
            }
        }

        private void DrawIData(IData data)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"🔹 {data.GetType().Name})", EditorStyles.miniBoldLabel);

            FieldInfo[] fields = data.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(data);
                EditorGUILayout.LabelField($"  {field.Name}: {value}");
            }

            EditorGUILayout.EndVertical();
        }
    }
}
