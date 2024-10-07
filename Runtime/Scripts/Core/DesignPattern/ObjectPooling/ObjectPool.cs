using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class ObjectPool<T>
    {
        private List<ObjectPoolContainer<T>> _listPool;
        private Dictionary<T, ObjectPoolContainer<T>> _lookupDic;
        private Func<T> _factoryFunc;
        private int _lastIndex = 0;
        public int Count
        {
            get => _listPool.Count;
        }

        public int CountUsedItems
        {
            get => _lookupDic.Count;
        }

        public string Group;

        public ObjectPool(Func<T> factoryFunc, int initialSize)
        {
            this._factoryFunc = factoryFunc;
            _listPool = new List<ObjectPoolContainer<T>>(initialSize);
            _lookupDic = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);
            Warm(initialSize);
        }

        private void Warm(int capacity)
        {
            for (int i = 0; i < capacity; i++)
                CreateContainer();
        }

        private ObjectPoolContainer<T> CreateContainer()
        {
            var container = new ObjectPoolContainer<T>();
            container.Item = _factoryFunc();
            _listPool.Add(container);
            return container;
        }

        public T GetItem()
        {
            ObjectPoolContainer<T> container = null;
            for (int i = 0; i < _listPool.Count; i++)
            {
                _lastIndex++;
                if (_lastIndex > _listPool.Count - 1) _lastIndex = 0;

                if (_listPool[_lastIndex].Used)
                {
                    continue;
                }
                else
                {
                    container = _listPool[_lastIndex];
                    break;
                }
            }

            container ??= CreateContainer();
            container.Consume();

            _lookupDic.Add(container.Item, container);
            return container.Item;
        }
        
        public List<T> GetAllItems()
        {
            List<T> items = new List<T>();
            foreach (var container in _listPool)
            {
                items.Add(container.Item);
            }
            return items;
        }


        public void ReleaseItem(T item)
        {
            if (_lookupDic.TryGetValue(item, out var container))
            {
                container.Release();
                _lookupDic.Remove(item);
            }
            else
            {
                Debug.LogWarning("This object pool does not contain the item provided: " + item);
            }
        }
    }
}
