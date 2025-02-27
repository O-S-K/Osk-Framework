using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class DataManager : GameFrameworkComponent
    {  
        [SerializeReference, SerializeField]
        private Dictionary<Type, List<IData>> k_DataStore = new Dictionary<Type, List<IData>>();

        public override void OnInit() {}

        // add data to the data store
        public void Add<T>(T data) where T : class, IData
        {
            Type type = typeof(T);
            if (!k_DataStore.ContainsKey(type))
            {
                Logg.Log($"Creating data store for type {type}.", Color.green);
                k_DataStore[type] = new List<IData>();
            }

            List<IData> list = k_DataStore[type];

            if (!list.Contains(data))
            {
                Logg.Log($"Adding data of type {type}.", Color.green);
                list.Add(data);
            }
            else
            {
                Logg.LogWarning($"Data of type {type} already exists.");
            }
        }

        // add data to the data store
        public void Add<T>(List<T> dataList) where T : class, IData
        {
            Type type = typeof(T);
            if (!k_DataStore.ContainsKey(type))
            {
                k_DataStore[type] = new List<IData>();
            }

            Logg.Log($"Adding list of type {type}.", Color.green);
            k_DataStore[type].AddRange(dataList);
        }

        // get all data of type T
        public List<T> GetAll<T>() where T : class, IData
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", Color.green);
                return value.ConvertAll(x => x as T);
            }

            Logg.LogWarning($"No data found of type {type}.");
            return new List<T>();
        }

        // get first data of type T
        public T Get<T>() where T : class, IData
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value) && value.Count > 0)
            {
                Logg.Log($"Data of type {type} found.", Color.green);
                return value[0] as T;
            }

            Logg.LogWarning($"No data found of type {type}.");
            return null;
        }

        // find data of type T
        public T Query<T>(Predicate<T> query) where T : class, IData
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Querying data of type {type}.", Color.green);
                return value.ConvertAll(x => x as T).Find(query);
            }

            Logg.LogWarning($"No data found of type {type}.");
            return null;
        }

        // take all data of type T
        public List<T> QueriesAll<T>(Predicate<T> query) where T : class, IData
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Querying all data of type {type}.", Color.green);
                return value.ConvertAll(x => x as T).FindAll(query);
            }

            Logg.LogWarning($"No data found of type {type}.");
            return new List<T>();
        }
        
        public void Remove<T>(T data) where T : class, IData
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Removing data of type {type}.");
                value.Remove(data);
            }
        }

        // remove data of type T
        public void Remove<T>(Predicate<T> query) where T : class, IData
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Removing data of type {type}.");
                value.RemoveAll(x => query((T)x));
            }
        }
    }
}
