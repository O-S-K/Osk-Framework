using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class PoolManager : GameFrameworkComponent
    {
        [SerializeReference] 
        public Dictionary<string, Dictionary<Object, ObjectPool<Object>>> k_GroupPrefabLookup = new();

        [SerializeReference]
        public Dictionary<Object, ObjectPool<Object>> k_InstanceLookup = new Dictionary<Object, ObjectPool<Object>>();

        private Dictionary<string, GameObject> k_GroupObjects = new Dictionary<string, GameObject>();

        public override void OnInit()
        {
        }

        public void Preload(string groupName, Object prefab, Transform parent, int size)
        {
            WarmPool(groupName, prefab, parent, size);
        }

        public T Query<T>(string groupName, T prefab) where T : Object
        {
            if (k_GroupPrefabLookup.TryGetValue(groupName, out var prefabPools))
            {
                if (prefabPools.TryGetValue(prefab, out var pool))
                {
                    return pool.GetItem() as T;
                }
            }
            return null;
        }
        
        public T Spawn<T>(string groupName, T prefab, Transform parent) where T : Object
        {
            return Spawn(groupName, prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public T Spawn<T>(string groupName, T prefab, Transform parent, Transform transform) where T : Object
        {
            return Spawn(groupName, prefab, parent, transform.position, Quaternion.identity);
        }

        public T Spawn<T>(string groupName, T prefab, Transform parent, Transform transform, Quaternion rotation)
            where T : Object
        {
            return Spawn(groupName, prefab, parent, transform.position, rotation);
        }

        
        public T Spawn<T>(string groupName, T prefab, Transform parent, Vector3 position)
            where T : Object
        {
            return Spawn(groupName, prefab, parent, position, Quaternion.identity);
        }
        
        public T Spawn<T>(string groupName, T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Object
        {
            var s = Spawn(groupName, prefab, parent, 1);
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

        public T Spawn<T>(string groupName, T prefab, Transform parent, int size) where T : Object
        {
            GetOrCreateGroup(groupName, parent);
            if (!IsGroupAndPrefabExist(groupName, prefab))
            {
                if(size <= 0)
                {
                    Logg.LogError("Pool size must be greater than 0.");
                    return null;
                }
                WarmPool(groupName, prefab, parent, size);
            }

            var pool = k_GroupPrefabLookup[groupName][prefab];
            var instance = pool.GetItem() as T;
            if (instance is Component component)
            {
                component.gameObject.SetActive(true);
                component.transform.SetParent(GetOrCreateGroup(groupName, parent).transform);
            }
            else if (instance is GameObject go)
            {
                go.SetActive(true);
                go.transform.SetParent(GetOrCreateGroup(groupName, parent).transform);
            }

            if (!k_InstanceLookup.TryAdd(instance, pool))
            {
                Logg.LogWarning($"This object pool already contains the item provided: {instance}");
                return instance;
            }

            return instance;
        }

        private void WarmPool<T>(string group, T prefab, Transform parent, int size) where T : Object
        {
            if (IsGroupAndPrefabExist(group, prefab))
            {
                Logg.LogError( $"Pool for prefab '{prefab.name}' in group '{group}' has already been created.");
            }
            if(size <= 0)
            {
                Logg.LogError("Pool size must be greater than 0.");
                return;
            }
            var groupObject = GetOrCreateGroup(group, parent);
            var pool = new ObjectPool<Object>(() => InstantiatePrefab(prefab, groupObject.transform), size);
            pool.Group = group;

            if (!k_GroupPrefabLookup.ContainsKey(group))
            {
                k_GroupPrefabLookup[group] = new Dictionary<Object, ObjectPool<Object>>();
            }

            k_GroupPrefabLookup[group][prefab] = pool;
        }

        public bool HasGroup(string groupName)
        {
            return k_GroupObjects.ContainsKey(groupName);
        }
        
        private Object InstantiatePrefab<T>(T prefab, Transform parent) where T : Object
        {
            return prefab is GameObject gameObjectPrefab
                ? Instantiate(gameObjectPrefab, parent)
                : Instantiate((Component)(object)prefab, parent);
        }

        private GameObject GetOrCreateGroup(string groupName, Transform parent)
        {
            if (!k_GroupObjects.TryGetValue(groupName, out var groupObject))
            {
                groupObject = new GameObject(groupName);
                
                 if (groupObject.transform.parent != parent)
                    groupObject.transform.SetParent(parent);
                k_GroupObjects[groupName] = groupObject;
            }
            return groupObject;
        }
        
        public void SetLocalScale(string groupName, Vector3 scale)
        {
            if(k_GroupObjects.TryGetValue(groupName, out var groupObject))
            {
                groupObject.transform.localScale = scale;
            }
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