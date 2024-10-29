using System;
using UnityEngine;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace OSK
{
    public interface IService
    {
    }

    public class ServiceLocator : GameFrameworkComponent
    {
        private readonly Dictionary<Type, IService> k_Services = new Dictionary<Type, IService>();
        private readonly Dictionary<Type, Action<IService>> k_Callbacks = new();
        private readonly Dictionary<Delegate, Action<IService>> k_RegisteredCallbacks = new();
 
        public override void OnInit() {}

        
        public bool Register<T>(T service) where T : class, IService
        {
            var type = typeof(T);

            if (!k_Services.TryAdd(type, service))
            {
                Logg.LogError($"Service of type {type.Name} already registered");
                return false;
            }

            if (k_Callbacks.TryGetValue(type, out var serviceCallback))
            {
                serviceCallback?.Invoke(service);
            }

            return true;
        }

        public bool Unregister<T>(T service) where T : class, IService
        {
            var type = typeof(T);

            if (k_Services.TryGetValue(type, out var temp) && service == temp)
            {
                k_Services.Remove(type);
                return true;
            }

            Logg.Log($"This instance of {type.Name} is not registered");
            return false;
        }

        public T Get<T>() where T : class, IService
        {
            if (k_Services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            Logg.LogError($"Service of type {typeof(T).Name} not found");
            return null;
        }

        public bool TryGet<T>(out T result) where T : class, IService
        {
            if (k_Services.TryGetValue(typeof(T), out var service))
            {
                result = (T)service;
                return true;
            }

            result = default;
            return false;
        }

        public void AddServiceListener<T>(Action<T> serviceCallback) where T : class, IService
        {
            if (k_RegisteredCallbacks.ContainsKey(serviceCallback))
            {
                Debug.LogWarning("double registered");
                return;
            }

            if (TryGet<T>(out var service))
            {
                serviceCallback?.Invoke(service);
                return;
            }

            var type = typeof(T);

            Action<IService> newAction = (e) => serviceCallback((T)e);

            if (k_Callbacks.TryGetValue(type, out var result))
            {
                k_Callbacks[type] = result += newAction;
            }
            else
            {
                k_Callbacks[type] = newAction;
            }

            k_RegisteredCallbacks.Add(serviceCallback, newAction);
        }

        public void RemoveServiceListener<T>(Action<T> serviceCallback) where T : class, IService
        {
            if (!k_RegisteredCallbacks.TryGetValue(serviceCallback, out var action))
            {
                return;
            }

            var type = typeof(T);

            if (k_Callbacks.TryGetValue(type, out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                {
                    k_Callbacks.Remove(type);
                }
                else
                {
                    k_Callbacks[type] = tempAction;
                }
            }

            k_RegisteredCallbacks.Remove(serviceCallback);
        }
    }
}