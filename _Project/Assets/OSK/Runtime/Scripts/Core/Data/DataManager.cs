using System;
using System.Collections.Generic;
using System.IO;
using CustomInspector;
using Newtonsoft.Json;
using UnityEngine;

namespace OSK
{
    public class DataManager : GameFrameworkComponent
    {
        // Data store
        [SerializeField] private SerializeFieldDictionary<Type, object> _dataStore = new();

        // Add data to the data store
        public void Add<T>(T data)
        {
            Type type = typeof(T);
            if (!_dataStore.ContainsKey(type))
            {
                Logg.Log($"Creating data store for type {type}.", ColorCustom.Green);
                _dataStore[type] = new List<T>();
            }

            // check if data is already in the list
            List<T> list = (List<T>)_dataStore[type];
            if (!list.Contains(data))
            {
                Logg.Log($"Adding data of type {type}.", ColorCustom.Green);
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
            if (!_dataStore.ContainsKey(type))
            {
                _dataStore[type] = new List<T>();
            }

            Logg.Log($"Adding data of type {type}.", ColorCustom.Green);
            ((List<T>)_dataStore[type]).AddRange(dataList);
        }

        // Get all data of type T
        public List<T> GetAll<T>()
        {
            Type type = typeof(T);
            if (_dataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", ColorCustom.Green);
                return (List<T>)value;
            }

            Logg.LogWarning($"No data found of type {type}.");
            return new List<T>();
        }

        public T Get<T>()
        {
            Type type = typeof(T);
            if (_dataStore.TryGetValue(type, out var value))
            {
                List<T> list = (List<T>)value;
                if (list.Count > 0)
                {
                    Logg.Log($"Data of type {type} found.", ColorCustom.Green);
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
            if (_dataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", ColorCustom.Green);
                List<T> list = (List<T>)value;
                return list.Find(query);
            }

            Logg.LogWarning($"No data found of type {type}.");
            return default;
        }
        
        public List<T> QueriesAll<T>(Predicate<T> query)
        {
            Type type = typeof(T);
            if (_dataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.", ColorCustom.Green);
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
            if (_dataStore.TryGetValue(type, out var value))
            {
                Logg.Log($"Data of type {type} found.");
                List<T> list = (List<T>)value;
                list.RemoveAll(query);
            }
        }

        public void SaveToFile<T>(string filePath)
        {
            Type type = typeof(T);
            if (_dataStore.TryGetValue(type, out var value))
            {
                List<T> list = (List<T>)value;
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Logg.Log($"Data of type {type} saved to {filePath}.");
            }
            else
            {
                Logg.LogWarning($"No data found of type {type} to save.");
            }
        }

        public void LoadFromFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<T> dataList = JsonConvert.DeserializeObject<List<T>>(json);
                if (dataList != null)
                {
                    Type type = typeof(T);
                    _dataStore[type] = dataList;
                    Logg.Log($"Data of type {type} loaded from {filePath}.");
                }
            }
            else
            {
                Logg.LogError($"File {filePath} does not exist.");
            }
        }
    }
}