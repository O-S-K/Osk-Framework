using System;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class DataManager : GameFrameworkComponent
    {  
        // Data store
        [SerializeReference] [SerializeField]
        private Dictionary<Type, object> k_DataStore = new Dictionary<Type, object>();
        
        public override void OnInit() {}

        
        // Add data to the data store
        public void Add<T>(T data)
        {
            Type type = typeof(T);
            if (!k_DataStore.ContainsKey(type))
            {
                Logg.Log($"Creating data store for type {type}.", Color.green);
                k_DataStore[type] = new List<T>();
            }

            // check if data is already in the list
            List<T> list = (List<T>)k_DataStore[type];
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

        // Add data to the data store
        public void Add<T>(List<T> dataList)
        {
            Type type = typeof(T);
            if (!k_DataStore.ContainsKey(type))
            {
                k_DataStore[type] = new List<T>();
            }

            Logg.Log($"Adding data of type {type}.", Color.green);
            ((List<T>)k_DataStore[type]).AddRange(dataList);
        }

        // Get all data of type T
        public List<T> GetAll<T>()
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", Color.green);
                return (List<T>)value;
            }

            Logg.LogWarning($"No data found of type {type}.");
            return new List<T>();
        }

        public T Get<T>()
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                List<T> list = (List<T>)value;
                if (list.Count > 0)
                {
                    Logg.Log($"Data of type {type} found.", Color.green);
                    return list[0];
                }

                Logg.LogWarning($"No data found of type {type}.");
            }

            return default;
        }

        // Query data with a condition
        public T Query<T>(Predicate<T> query)
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", Color.green);
                List<T> list = (List<T>)value;
                return list.Find(query);
            }

            Logg.LogWarning($"No data found of type {type}.");
            return default;
        }
        
        public List<T> QueriesAll<T>(Predicate<T> query)
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", Color.green);
                List<T> list = (List<T>)value;
                return list.FindAll(query);
            }

            Logg.LogWarning($"No data found of type {type}.");
            return new List<T>();
        }

        // Remove data based on a query
        public void Remove<T>(Predicate<T> query)
        {
            Type type = typeof(T);
            if (k_DataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.");
                List<T> list = (List<T>)value;
                list.RemoveAll(query);
            }
        } 
    }
}