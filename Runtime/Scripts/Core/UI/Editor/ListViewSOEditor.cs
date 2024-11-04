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
        ListViewSO listViewSO = null;
        private void OnEnable()
        {
             listViewSO = (ListViewSO)target;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            // Display header
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Depth", GUILayout.Width(50));
            EditorGUILayout.LabelField("Type", GUILayout.Width(100));
            EditorGUILayout.LabelField("Script Ref", GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
            
            // Display the list of views
            DisplayGroupedViews(listViewSO);
            
            EditorGUILayout.Space(10);

            // Button to add all views from Resources
            if (GUILayout.Button("Add All View From Resources"))
            {
                listViewSO.AddAllViewFormResources();
                EditorUtility.SetDirty(target);
            }

            // Button to sort by ViewType and Depth
            if (GUILayout.Button("Sort By ViewType and Depth"))
            {
                SortViews(listViewSO);
            }
            
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Add"))
            {
                listViewSO.Views.Add(new DataViewUI { depth = "0", view = null });
                EditorUtility.SetDirty(target);
            }
            
            if (GUILayout.Button("Refresh"))
            {
                EditorUtility.SetDirty(target);
            }
            
            if(GUILayout.Button("Clear All"))
            {
                listViewSO.Views.Clear();
                EditorUtility.SetDirty(target);
            }


            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void SortViews(ListViewSO listViewSO)
        {
            listViewSO.Views.Sort((a, b) =>
            {
                int viewTypeComparison = a.view.viewType.CompareTo(b.view.viewType);
                if (viewTypeComparison == 0)
                {
                    return int.Parse(a.depth).CompareTo(int.Parse(b.depth));
                }
                return viewTypeComparison;
            });

            EditorUtility.SetDirty(listViewSO);
            Debug.Log("Sorted views by ViewType and Depth");
        }

        private void DisplayGroupedViews(ListViewSO listViewSO)
        {
            EViewType? currentViewType = null;

            for (int i = 0; i < listViewSO.Views.Count; i++)
            {
                var dataView = listViewSO.Views[i];

                // Check for new viewType group
                if (dataView.view != null)
                {
                    // Check if we've encountered a new viewType
                    if (currentViewType != dataView.view.viewType)
                    {
                        currentViewType = dataView.view.viewType;

                        // Initialize foldout state for this viewType if not already done
                        if (!viewTypeFoldouts.ContainsKey(currentViewType.Value))
                        {
                            viewTypeFoldouts[currentViewType.Value] = true; // Default to open
                        }

                        EditorGUILayout.Space();
                        // Foldout for view type
                        viewTypeFoldouts[currentViewType.Value] = EditorGUILayout.Foldout(viewTypeFoldouts[currentViewType.Value], $"{currentViewType}", true, EditorStyles.foldout);
                    }

                    // Display the data row only if the foldout is open
                    if (currentViewType.HasValue && viewTypeFoldouts[currentViewType.Value])
                    {
                        DisplayViewRow(dataView, listViewSO, i);
                    }
                }
                else
                {
                    currentViewType = null; 
                    dataView.view = (View)EditorGUILayout.ObjectField(dataView.view, typeof(View), allowSceneObjects: false, GUILayout.Width(200));
                    // Remove button
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        listViewSO.Views.Remove(dataView);
                        EditorUtility.SetDirty(target);
                        return; // Exit to avoid issues with list modification
                    }

                }
            }
        }

        private void DisplayViewRow(DataViewUI dataView, ListViewSO listViewSO, int index)
        {
            EditorGUILayout.BeginHorizontal();

            // Editable Depth column
            dataView.depth = EditorGUILayout.IntField(int.Parse(dataView.depth), GUILayout.Width(50)).ToString();

            // Type column as EnumPopup
            if (dataView.view != null)
            {
                dataView.view.viewType = (EViewType)EditorGUILayout.EnumPopup(dataView.view.viewType, GUILayout.Width(100));
            }
            else
            {
                EditorGUILayout.LabelField("N/A", GUILayout.Width(100));
            }

            // Script Ref column as ObjectField
            dataView.view = (View)EditorGUILayout.ObjectField(dataView.view, typeof(View), allowSceneObjects: false, GUILayout.Width(200));

            // Check for duplicate
            if (dataView.view != null)
            {
                // Check if the current view already exists in the list
                bool isDuplicate = listViewSO.Views.Count(v => v.view == dataView.view) > 1;

                // If duplicate exists, show the duplicate field
                if (isDuplicate)
                {
                    EditorGUILayout.LabelField("Duplicate Exists", GUILayout.Width(100));
                }
            }

            // Remove button
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                listViewSO.Views.RemoveAt(index);
                EditorUtility.SetDirty(target);
                return; // Exit to avoid issues with list modification
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
