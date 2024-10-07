using System;
using UnityEngine;
using System.Collections.Concurrent;


namespace OSK
{
public  class ServiceLocator 
{
    private static readonly ConcurrentDictionary<Type, MonoBehaviour> _services = new();

    public static void Register<T>(T service) where T : MonoBehaviour
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            throw new Exception($"Service of type {type} already registered");
        }
        else
        {
            _services[type] = service;
            OSK.Logg.Log("Service registered: " + type);
        }
    }

    public static T Get<T>() where T : MonoBehaviour
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }
        throw new Exception( $"Service of type {type} not found");
    }

    public static void Unregister<T>() where T : MonoBehaviour
    {
        var type = typeof(T);
        _services.TryRemove(type, out _);
        OSK.Logg.Log("Service unregistered: " + type);
    }
}
}