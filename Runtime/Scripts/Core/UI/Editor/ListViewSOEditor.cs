#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace OSK
{
    [CustomEditor(typeof(ListViewSO))]
    public class ListViewSOEditor : Editor
    {
        private Dictionary<EViewType, bool> viewTypeFoldouts = new Dictionary<EViewType, bool>();
        private ListViewSO listViewSO = null;
        
        private void OnEnable() => listViewSO = (ListViewSO)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Depth", GUILayout.Width(50));
            EditorGUILayout.LabelField("Type", GUILayout.Width(100));
            EditorGUILayout.LabelField("Script Ref", GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("----------------------------------------------------------------------------");


            DisplayGroupedViews(listViewSO);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("----------------------------------------------------------------------------");
            EditorGUILayout.Space();

            if (GUILayout.Button("Add All View From Resources"))
            {
                AddAllViewFormResources();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Sort By ViewType and Depth"))
            {
                SortViews(listViewSO);
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Add"))
            {
                listViewSO.Views.Add(new DataViewUI { depth = 0, view = null });
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Refresh"))
            {
                RefreshListUI();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Clear All"))
            {
                listViewSO.Views.Clear();
                EditorUtility.SetDirty(target);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void CheckUniqueUI()
        {
            var views = new List<DataViewUI>();
            foreach (var view in listViewSO.Views)
            {
                if (views.Contains(view))
                    OSK.Logg.LogError($"Popup Type {view} exists in the list. Please remove it.");
                else
                    views.Add(view);
            }
        }

        public void AddAllViewFormResources()
        {
            var listViews = Resources.LoadAll<View>("").ToList().FindAll(x => x.isAddToViewManager);
            if(listViews.Count == 0)
            {
                OSK.Logg.LogWarning("No views found in Resources folder");
                return;
            }
            
            foreach (var popup in listViews)
            {
                if (listViewSO.Views.Any(x => x.view == popup))
                    continue;

                var data = new DataViewUI
                {
                    view = popup,
                    path = IOUtility.GetPathAfterResources(popup)
                };
                data.depth = popup.depth;
                listViewSO.Views.Add(data);
            }
            SortViews(listViewSO);
        }
        
        private void RefreshListUI()
        {
            SortViews(listViewSO);
        }
 

        private void SortViews(ListViewSO listViewSO)
        {
            listViewSO.Views.Sort((x, y) =>
            {
                int depthComparison = x.depth.CompareTo(y.depth);
                if (depthComparison != 0)
                {
                    return depthComparison;
                }
                return x.view.viewType.CompareTo(y.view.viewType);
            });

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(target);
        }

        private void DisplayGroupedViews(ListViewSO listViewSO)
        {
            EViewType? currentViewType = null;

            for (int i = 0; i < listViewSO.Views.Count; i++)
            {
                var dataView = listViewSO.Views[i];

                if (dataView.view != null)
                {
                    if (currentViewType != dataView.view.viewType)
                    {
                        currentViewType = dataView.view.viewType;

                        if (!viewTypeFoldouts.ContainsKey(currentViewType.Value))
                        {
                            viewTypeFoldouts[currentViewType.Value] = true; 
                        }

                        EditorGUILayout.Space();
                        viewTypeFoldouts[currentViewType.Value] = EditorGUILayout.Foldout(
                            viewTypeFoldouts[currentViewType.Value], $"{currentViewType}", true, EditorStyles.foldout);
                    }

                    if (currentViewType.HasValue && viewTypeFoldouts[currentViewType.Value])
                    {
                        DisplayViewRow(dataView, listViewSO, i);
                    }
                }
                else
                {
                    currentViewType = null;
                    dataView.view = (View)EditorGUILayout.ObjectField(dataView.view, typeof(View),
                        allowSceneObjects: false, GUILayout.Width(200));
                    
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        listViewSO.Views.Remove(dataView);
                        EditorUtility.SetDirty(target);
                        return; 
                    }
                }
            }
        }

        private void DisplayViewRow(DataViewUI dataView, ListViewSO listViewSO, int index)
        {
            EditorGUILayout.BeginHorizontal();
            dataView.depth = EditorGUILayout.IntField(dataView.depth, GUILayout.Width(50));

            if (dataView.view != null)
            {
                dataView.view.viewType =
                    (EViewType)EditorGUILayout.EnumPopup(dataView.view.viewType, GUILayout.Width(100));
            }
            else
            {
                EditorGUILayout.LabelField("N/A", GUILayout.Width(100));
            }

            dataView.view = (View)EditorGUILayout.ObjectField(dataView.view, typeof(View), allowSceneObjects: false,
                GUILayout.Width(200));

            if (dataView.view != null)
            {
                bool isDuplicate = listViewSO.Views.Count(v => v.view == dataView.view) > 1;

                if (isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Exists", GUILayout.Width(100));
                }
            }

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                listViewSO.Views.RemoveAt(index);
                EditorUtility.SetDirty(target);
                return; 
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif