using System;
using UnityEngine;
using System.Collections.Concurrent;


public  class ServiceLocator 
{
    private static readonly ConcurrentDictionary<Type, MonoBehaviour> _services = new();

    public static void RegisterService<T>(T service) where T : MonoBehaviour
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            throw new Exception($"Service of type {type} already registered");
        }
        else
        {
            _services[type] = service;
            Debug.Log("Service registered: " + type);
        }
    }

    public static T GetService<T>() where T : MonoBehaviour
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }
        throw new Exception($"Service of type {type} not found");
    }

    public static void UnregisterService<T>() where T : MonoBehaviour
    {
        var type = typeof(T);
        _services.TryRemove(type, out _);
        Debug.Log("Service unregistered: " + type);
    }
}