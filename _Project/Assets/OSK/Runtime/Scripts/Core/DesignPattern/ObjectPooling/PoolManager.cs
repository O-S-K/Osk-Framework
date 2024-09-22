using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class PooleData
    {
        public Component prefab;
        public int size;
        public int maxCapacity;
    }
    
    [System.Serializable]
    public class PoolGroup
    {
        public string groupName;
        public PooleData[] pooleDatas;
    }

    public class PoolManager : GameFrameworkComponent
    {
        // Dictionary to store pools, keyed by group name and prefab
        public Dictionary<string, Dictionary<Component, ObjectPool<Component>>> groupPrefabLookup = new();
        public Dictionary<Component, ObjectPool<Component>> instanceLookup = new();

        [HideInInspector] public bool dirty = false;
        private Dictionary<string, GameObject> groupObjects = new();


        /// <summary>
        /// Warms the pool for a specific prefab within a group.
        /// </summary>
        public void WarmPool<T>(string group, T prefab, int size) where T : Component
        {
            if (IsGroupAndPrefabExist(group, prefab))
            {
                throw new Exception($"Pool for prefab '{prefab.name}' in group '{group}' has already been created.");
            }

            var groupObject = GetOrCreateGroup(group);
            
            // Create a new pool and assign it to the correct group
            var pool = new ObjectPool<Component>(() => Instantiate(prefab, groupObject.transform), size);
            pool.Group = group;

            if (!groupPrefabLookup.ContainsKey(group))
            {
                groupPrefabLookup[group] = new Dictionary<Component, ObjectPool<Component>>();
            }

            groupPrefabLookup[group][prefab] = pool;
            dirty = true;
        }

        /// <summary>
        /// Creates or retrieves an instance of a prefab within a group.
        /// </summary>
        public T Create<T>(string groupName, T prefab, int size = 1) where T : Component
        {
            // Ensure the group exists
            var groupObject = GetOrCreateGroup(groupName);

            // Check if the prefab has an associated pool in the group, if not, create it
            if (!IsGroupAndPrefabExist(groupName, prefab))
            {
                WarmPool(groupName, prefab, size);
            }

            // Retrieve the prefab's pool and instantiate
            var pool = groupPrefabLookup[groupName][prefab];
            var clone = pool.GetItem() as T;
            clone.gameObject.SetActive(true);
            clone.transform.parent = groupObject.transform;  // Ensure the object is correctly parented to the group
            instanceLookup.Add(clone, pool);
            dirty = true;
            return clone;
        }

        
        /// <summary>
        /// Creates or retrieves the group GameObject for pooling objects.
        /// </summary>
        public GameObject GetOrCreateGroup(string groupName)
        {
            if (groupObjects.ContainsKey(groupName))
            {
                return groupObjects[groupName];
            }

            // Create a new group if it doesn't exist
            var group = new GameObject(groupName);
            group.transform.SetParent(transform);  // Parent the group under the PoolManager
            groupObjects[groupName] = group;
            return group;
        }

        /// <summary>
        /// Checks if both the group and prefab exist in the pool.
        /// </summary>
        public bool IsGroupAndPrefabExist(string groupName, Component prefab)
        {
            return groupPrefabLookup.ContainsKey(groupName) && groupPrefabLookup[groupName].ContainsKey(prefab);
        }

        public void Release(Component clone)
        {
            clone.gameObject.SetActive(false);

            if (instanceLookup.ContainsKey(clone))
            {
                instanceLookup[clone].ReleaseItem(clone);
                instanceLookup.Remove(clone);
                dirty = true;
            }
            else
            {
                Debug.LogWarning("No pool contains the object: " + clone.name);
            }
        }
        
        public void ReleaseAllObjectInGroup(string groupName)
        {
            if (groupPrefabLookup.ContainsKey(groupName))
            {
                foreach (var keyVal in groupPrefabLookup[groupName])
                {
                    keyVal.Key.gameObject.SetActive(false);
                    keyVal.Value.ReleaseItem(keyVal.Key);
                    instanceLookup.Clear();
                    dirty = true;
                }
 
            }
        }

        public void ReleaseAllObjectActive()
        {
            foreach (var keyVal in instanceLookup)
            {
                keyVal.Key.gameObject.SetActive(false);
                keyVal.Value.ReleaseItem(keyVal.Key);
            }

            instanceLookup.Clear();
            dirty = true;
        }

        public void DestroyGroup(string groupName)
        {
            if (groupObjects.ContainsKey(groupName))
            {
                Destroy(groupObjects[groupName]);
                groupObjects.Remove(groupName);
                groupPrefabLookup.Remove(groupName);
                dirty = true;
            }
        }
        
        public void DestroyAllGroups()
        {
            foreach (var group in groupObjects)
            {
                Destroy(group.Value);
            }

            groupPrefabLookup.Clear();
            groupObjects.Clear();
            dirty = true;
        }
    }
}
