using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using OSK;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    private PoolManager poolManager;

    private void OnEnable()
    {
        poolManager = (PoolManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Object Pool Hierarchy", EditorStyles.boldLabel);

        if (poolManager.groupPrefabLookup.Count > 0)
        {
            DisplayGroupedPools();
        }
        else
        {
            EditorGUILayout.LabelField("No pools are currently initialized.");
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DisplayGroupedPools()
    {
        Dictionary<string, int> prefabCountLookup = new Dictionary<string, int>();

        // Gather prefab counts by name
        foreach (var groupEntry in poolManager.groupPrefabLookup)
        {
            Dictionary<Component, ObjectPool<Component>> prefabPools = groupEntry.Value;

            foreach (var prefabEntry in prefabPools)
            {
                Component prefab = prefabEntry.Key;
                ObjectPool<Component> pool = prefabEntry.Value;
                int count = pool.Count;

                if (prefabCountLookup.ContainsKey(prefab.name))
                {
                    prefabCountLookup[prefab.name] += count;
                }
                else
                {
                    prefabCountLookup[prefab.name] = count;
                }
            }
        }

        // Display grouped pools with total counts
        EditorGUILayout.Space();
        foreach (var groupEntry in poolManager.groupPrefabLookup)
        {
            string groupName = groupEntry.Key;
            EditorGUILayout.LabelField($"Group: {groupName}", EditorStyles.boldLabel);

            foreach (var prefabEntry in prefabCountLookup)
            {
                string prefabName = prefabEntry.Key;
                int totalCount = prefabEntry.Value;

                EditorGUILayout.LabelField($"Prefab: {prefabName} - Total Count: {totalCount}");
            }
        }
    }
}