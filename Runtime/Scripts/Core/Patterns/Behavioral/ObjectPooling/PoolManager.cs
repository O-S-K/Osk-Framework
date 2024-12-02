using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class PoolManager : GameFrameworkComponent
    {
        [SerializeReference] public Dictionary<string, Dictionary<Object, ObjectPool<Object>>> k_GroupPrefabLookup =
            new Dictionary<string, Dictionary<Object, ObjectPool<Object>>>();

        [SerializeReference]
        public Dictionary<Object, ObjectPool<Object>> k_InstanceLookup = new Dictionary<Object, ObjectPool<Object>>();

        private Dictionary<string, GameObject> k_GroupObjects = new Dictionary<string, GameObject>();

        public override void OnInit()
        {
        }

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

        public T Spawn<T>(string groupName, T prefab, Transform position) where T : Object
        {
            return Spawn(groupName, prefab, position.position, Quaternion.identity);
        }

        public T Spawn<T>(string groupName, T prefab, Transform position, Quaternion rotation) where T : Object
        {
            return Spawn(groupName, prefab, position.position, rotation);
        }

        public T Spawn<T>(string groupName, T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            var s = Spawn(groupName, prefab, 1);
            if (s is Component component)
            {
                component.transform.position = position;
                component.transform.rotation = rotation;
            }
            else if (s is GameObject go)
            {
                go.transform.position = position;
                go.transform.rotation = rotation;
            }

            return s;
        }

        public T Spawn<T>(string groupName, T prefab, int size = 1) where T : Object
        {
            GetOrCreateGroup(groupName);
            if (!IsGroupAndPrefabExist(groupName, prefab))
            {
                WarmPool(groupName, prefab, size);
            }

            var pool = k_GroupPrefabLookup[groupName][prefab];
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

            if (!k_InstanceLookup.TryAdd(instance, pool))
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

            if (!k_GroupPrefabLookup.ContainsKey(group))
            {
                k_GroupPrefabLookup[group] = new Dictionary<Object, ObjectPool<Object>>();
            }

            k_GroupPrefabLookup[group][prefab] = pool;
        }

        private Object InstantiatePrefab<T>(T prefab, Transform parent) where T : Object
        {
            return prefab is GameObject gameObjectPrefab
                ? Instantiate(gameObjectPrefab, parent)
                : Instantiate((Component)(object)prefab, parent);
        }

        private GameObject GetOrCreateGroup(string groupName)
        {
            if (!k_GroupObjects.TryGetValue(groupName, out var groupObject))
            {
                groupObject = new GameObject(groupName);
                groupObject.transform.SetParent(transform);
                k_GroupObjects[groupName] = groupObject;
            }

            return groupObject;
        }

        private bool IsGroupAndPrefabExist(string groupName, Object prefab)
        {
            return k_GroupPrefabLookup.ContainsKey(groupName) && k_GroupPrefabLookup[groupName].ContainsKey(prefab);
        }

        public void Despawn(Object instance)
        {
            DeactivateInstance(instance);
            if (k_InstanceLookup.TryGetValue(instance, out var pool))
            {
                pool.ReleaseItem(instance);
                k_InstanceLookup.Remove(instance);
            }
            else
            {
                Logg.LogWarning($"{instance} not found in any pool.");
            }
        }

        public void Despawn(Object instance, float delay, bool unscaleTime = false)
        {
            this.DoDelay(delay, () =>
            {
                if (instance != null) Despawn(instance);
            }, unscaleTime);
        }

        public void DespawnAllInGroup(string groupName)
        {
            if (k_GroupPrefabLookup.TryGetValue(groupName, out var prefabPools))
            {
                foreach (var pool in prefabPools)
                {
                    DeactivateInstance(pool.Key);
                    pool.Value.ReleaseItem(pool.Key);
                }

                k_InstanceLookup.Clear();
            }
        }

        public void DespawnAllActive()
        {
            foreach (var keyVal in k_InstanceLookup)
            {
                DeactivateInstance(keyVal.Key);
                keyVal.Value.ReleaseItem(keyVal.Key);
            }

            k_InstanceLookup.Clear();
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
            if (k_GroupObjects.TryGetValue(groupName, out var groupObject))
            {
                Destroy(groupObject);
                k_GroupObjects.Remove(groupName);
                k_GroupPrefabLookup.Remove(groupName);
            }
        }

        public void DestroyAllGroups()
        {
            foreach (var group in k_GroupObjects.Values)
            {
                Destroy(group);
            }

            k_GroupPrefabLookup.Clear();
            k_GroupObjects.Clear();
        }
    }
}