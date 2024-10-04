using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class PoolManager : GameFrameworkComponent
    {
        // Dictionary to store pools, keyed by group name and prefab (could be a Component or GameObject)
        public Dictionary<string, Dictionary<Object, ObjectPool<Object>>> groupPrefabLookup = new();
        private Dictionary<Object, ObjectPool<Object>> instanceLookup = new();

        [HideInInspector] public bool dirty = false;
        private Dictionary<string, GameObject> groupObjects = new();

        /// <summary>
        /// Preloads a number of instances of a prefab (Component or GameObject) within a group.
        /// </summary>
        public void Preload(string groupName, Object prefab, int size)
        {
            WarmPool(groupName, prefab, size);
        }

        /// <summary>
        /// Gets an instance of a prefab (Component or GameObject) within a group.
        /// </summary> 
        public T Get<T>(string groupName, T prefab) where T : Object
        {
            if (IsGroupAndPrefabExist(groupName, prefab))
            {
                return Spawn(groupName, prefab);
            }

            return null;
        }
    
        /// <summary>
        /// Creates or retrieves an instance of a prefab (Component or GameObject) within a group.
        /// </summary>
        public T Spawn<T>(string groupName, T prefab, int size = 1) where T : Object
        {
            GetOrCreateGroup(groupName);
            if (!IsGroupAndPrefabExist(groupName, prefab))
            {
                WarmPool(groupName, prefab, size);
            }

            var pool = groupPrefabLookup[groupName][prefab];
            var instance = pool.GetItem() as T;
            if (instance is Component component)
            {
                component.gameObject.SetActive(true);
                component.transform.parent = GetOrCreateGroup(groupName).transform;
            }
            else if (instance is GameObject go)
            {
                go.SetActive(true);
                go.transform.parent = GetOrCreateGroup(groupName).transform;
            }

            if (!instanceLookup.TryAdd(instance, pool))
            {
                Logg.LogWarning($"This object pool already contains the item provided: {instance}");
                return instance;
            }
            dirty = true;
            return instance;
        }

        /// <summary>
        /// Warms the pool for a specific prefab (Component or GameObject) within a group.
        /// </summary>
        private void WarmPool<T>(string group, T prefab, int size) where T : Object
        {
            if (IsGroupAndPrefabExist(group, prefab))
                throw new System.Exception(
                    $"Pool for prefab '{prefab.name}' in group '{group}' has already been created.");

            var groupObject = GetOrCreateGroup(group);
            var pool = new ObjectPool<Object>(() => InstantiatePrefab(prefab, groupObject.transform), size);
            pool.Group = group;

            if (!groupPrefabLookup.ContainsKey(group))
            {
                groupPrefabLookup[group] = new Dictionary<Object, ObjectPool<Object>>();
            }

            groupPrefabLookup[group][prefab] = pool;
            dirty = true;
        }

        /// <summary>
        /// Instantiates the prefab under the specified parent.
        /// </summary>
        private Object InstantiatePrefab<T>(T prefab, Transform parent) where T : Object
        {
            return prefab is GameObject gameObjectPrefab
                ? Instantiate(gameObjectPrefab, parent)
                : Instantiate((Component)(object)prefab, parent);
        }

        /// <summary>
        /// Creates or retrieves the group GameObject for pooling objects.
        /// </summary>
        private GameObject GetOrCreateGroup(string groupName)
        {
            if (!groupObjects.TryGetValue(groupName, out var groupObject))
            {
                groupObject = new GameObject(groupName);
                groupObject.transform.SetParent(transform);
                groupObjects[groupName] = groupObject;
            }

            return groupObject;
        }

        /// <summary>
        /// Checks if both the group and prefab exist in the pool.
        /// </summary>
        private bool IsGroupAndPrefabExist(string groupName, Object prefab)
        {
            return groupPrefabLookup.ContainsKey(groupName) && groupPrefabLookup[groupName].ContainsKey(prefab);
        }

        /// <summary>
        /// Deactivates and returns an instance back to its pool.
        /// </summary>
        public void Despawn(Object instance)
        {
            DeactivateInstance(instance);
            if (instanceLookup.TryGetValue(instance, out var pool))
            {
                pool.ReleaseItem(instance);
                instanceLookup.Remove(instance);
                dirty = true;
            }
            else
            {
                Logg.LogWarning($"This object pool does not contain the item provided: {instance}");
            }
        }

        /// <summary>
        /// Deactivates and returns all objects in a specified group to their pool.
        /// </summary>
        public void DespawnAllInGroup(string groupName)
        {
            if (groupPrefabLookup.TryGetValue(groupName, out var prefabPools))
            {
                foreach (var pool in prefabPools)
                {
                    DeactivateInstance(pool.Key);
                    pool.Value.ReleaseItem(pool.Key);
                }

                instanceLookup.Clear();
                dirty = true;
            }
        }

        /// <summary>
        /// Deactivates and returns all active instances to their pools.
        /// </summary>
        public void DespawnAllActive()
        {
            foreach (var keyVal in instanceLookup)
            {
                DeactivateInstance(keyVal.Key);
                keyVal.Value.ReleaseItem(keyVal.Key);
            }

            instanceLookup.Clear();
            dirty = true;
        }

        /// <summary>
        /// Deactivates a specific instance.
        /// </summary>
        private void DeactivateInstance(Object instance)
        {
            if (instance is Component component)
            {
                component.gameObject.SetActive(false);
            }
            else if (instance is GameObject go)
            {
                go.SetActive(false);
            }
        }

        /// <summary>
        /// Destroys a specified group and all its objects.
        /// </summary>
        public void DestroyGroup(string groupName)
        {
            if (groupObjects.TryGetValue(groupName, out var groupObject))
            {
                Destroy(groupObject);
                groupObjects.Remove(groupName);
                groupPrefabLookup.Remove(groupName);
                dirty = true;
            }
        }

        /// <summary>
        /// Destroys all groups and clears pools.
        /// </summary>
        public void DestroyAllGroups()
        {
            foreach (var group in groupObjects.Values)
            {
                Destroy(group);
            }

            groupPrefabLookup.Clear();
            groupObjects.Clear();
            dirty = true;
        }
    }
}