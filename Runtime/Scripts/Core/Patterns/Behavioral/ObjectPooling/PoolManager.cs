using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class PoolManager : GameFrameworkComponent
    {
        [SerializeReference] public Dictionary<string, Dictionary<Object, ObjectPool<Object>>> groupPrefabLookup = new();
        [SerializeReference] public Dictionary<Object, ObjectPool<Object>> instanceLookup = new();
        private Dictionary<string, GameObject> groupObjects = new();

        public override void OnInit() {}

        public void Preload(string groupName, Object prefab, int size)
        {
            WarmPool(groupName, prefab, size);
        }

        public T Get<T>(string groupName, T prefab) where T : Object
        {
            if (IsGroupAndPrefabExist(groupName, prefab))
            {
                return Spawn(groupName, prefab);
            }

            return null;
        }
    
        public T Spawn<T>(string groupName, T prefab, int size = 1) where T : Object
        {
#if UNITY_EDITOR
            var group = GetOrCreateGroup(groupName);
            group.transform.position = new Vector2(0, -5.5f);
#else
            GetOrCreateGroup(groupName);
#endif
            if (!IsGroupAndPrefabExist(groupName, prefab))
            {
                WarmPool(groupName, prefab, size);
            }

            var pool = groupPrefabLookup[groupName][prefab];
            var instance = pool.GetItem() as T;
            if (instance is Component component)
            {
                component.gameObject.SetActive(true);
                component.transform.SetParent(GetOrCreateGroup(groupName).transform);
            }
            else if (instance is GameObject go)
            {
                go.SetActive(true);
                go.transform.SetParent(GetOrCreateGroup(groupName).transform);
            }

            if (!instanceLookup.TryAdd(instance, pool))
            {
                Logg.LogWarning($"This object pool already contains the item provided: {instance}");
                return instance;
            }
            return instance;
        }

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
        }

        private Object InstantiatePrefab<T>(T prefab, Transform parent) where T : Object
        {
            return prefab is GameObject gameObjectPrefab
                ? Instantiate(gameObjectPrefab, parent)
                : Instantiate((Component)(object)prefab, parent);
        }

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

        private bool IsGroupAndPrefabExist(string groupName, Object prefab)
        {
            return groupPrefabLookup.ContainsKey(groupName) && groupPrefabLookup[groupName].ContainsKey(prefab);
        }

        public void Despawn(Object instance)
        {
            DeactivateInstance(instance);
            if (instanceLookup.TryGetValue(instance, out var pool))
            {
                pool.ReleaseItem(instance);
                instanceLookup.Remove(instance);
            }
            else
            {
                Logg.LogWarning($"{instance} not found in any pool.");
            }
        }

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
            }
        }

        public void DespawnAllActive()
        {
            foreach (var keyVal in instanceLookup)
            {
                DeactivateInstance(keyVal.Key);
                keyVal.Value.ReleaseItem(keyVal.Key);
            }

            instanceLookup.Clear();
        }

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

        public void DestroyGroup(string groupName)
        {
            if (groupObjects.TryGetValue(groupName, out var groupObject))
            {
                Destroy(groupObject);
                groupObjects.Remove(groupName);
                groupPrefabLookup.Remove(groupName);
            }
        }
 
        public void DestroyAllGroups()
        {
            foreach (var group in groupObjects.Values)
            {
                Destroy(group);
            }

            groupPrefabLookup.Clear();
            groupObjects.Clear();
        }
    }
}