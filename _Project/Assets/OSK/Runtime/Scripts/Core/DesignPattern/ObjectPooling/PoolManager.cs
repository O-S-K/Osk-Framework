using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class PoolManager : GameFrameworkComponent
    {
        public Dictionary<Component, ObjectPool<Component>> prefabLookup = new();
        public Dictionary<Component, ObjectPool<Component>> instanceLookup = new();


        public bool logStatus;
        private bool dirty = false;

        public void WarmPool<T>(T prefab, int size) where T : Component
        {
            if (prefabLookup.ContainsKey(prefab))
            {
                throw new Exception("Pool for prefab " + prefab.name + " has already been created");
            }

            var pool = new ObjectPool<Component>(() => Instantiate(prefab), size);
            prefabLookup[prefab] = pool;
            dirty = true;
            //return pool as T;
        }

        #region Spawns

        public T Spawn<T>(T prefab) where T : Component
        {
            if (!prefabLookup.ContainsKey(prefab))
            {
                WarmPool(prefab, 1);
            }

            var pool = prefabLookup[prefab];
            var clone = pool.GetItem() as T;
            clone.gameObject.SetActive(true);
            clone.transform.parent = transform;
            instanceLookup.Add(clone, pool);
            dirty = true;
            return clone;
        }

        public T Spawn<T>(T prefab, Transform parrent) where T : Component
        {
            if (!prefabLookup.ContainsKey(prefab))
            {
                WarmPool(prefab, 1);
            }

            var pool = prefabLookup[prefab];
            var clone = pool.GetItem() as T;
            if (clone != null)
            {
                clone.gameObject.SetActive(true);
                clone.transform.SetParent(parrent);

                instanceLookup.Add(clone, pool);
                dirty = true;
                return clone;
            }
            return null;
        }

        public void RemoveItemInPool(Component component)
        {
            var pool = prefabLookup[component];
            instanceLookup.Remove(pool.GetItem());
        }

        #endregion

        public void Despawn(Component clone)
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

        public void DespawnAll()
        {
            foreach (var keyVal in instanceLookup)
            {
                keyVal.Key.gameObject.SetActive(false);
                keyVal.Value.ReleaseItem(keyVal.Key);
            }

            instanceLookup.Clear();
            dirty = true;
        }

        public void DestroyAll()
        {
            transform.DestroyAllChildren();
            prefabLookup.Clear();
            instanceLookup.Clear();
            dirty = true;
        }


#if UNITY_EDITOR
        private void Update()
        {
            if (logStatus && dirty)
            {
                PrintStatus();
                dirty = false;
            }
        }

        private void PrintStatus()
        {
            foreach (KeyValuePair<Component, ObjectPool<Component>> keyVal in prefabLookup)
            {
                Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name,
                    keyVal.Value.CountUsedItems, keyVal.Value.Count));
            }
        }
#endif
    }
}