using System;
using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    public interface IService { }

    public class ServiceLocatorManager : GameFrameworkComponent
    {
        private readonly Dictionary<Type, IService> k_Services = new Dictionary<Type, IService>();
        private readonly Dictionary<Type, Action<IService>> k_Callbacks = new Dictionary<Type, Action<IService>>();
        private readonly Dictionary<Delegate, Action<IService>> k_RegisteredCallbacks = new Dictionary<Delegate, Action<IService>>();
 
        public override void OnInit() {}
        
        public void Register<T>(T service) where T : class, IService
        {
            var type = typeof(T);

            if (!k_Services.TryAdd(type, service))
            {
                Logg.LogError($"Service of type {type.Name} already registered");
            }

            if (k_Callbacks.TryGetValue(type, out var serviceCallback))
            {
                serviceCallback?.Invoke(service);
            }
        }

        public void Unregister<T>(T service) where T : class, IService
        {
            var type = typeof(T);
            if (k_Services.TryGetValue(type, out var temp) && service == temp)
            {
                k_Services.Remove(type);
            }
            Logg.Log($"This instance of {type.Name} is not registered");
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
            }

            result = default;
            return false;
        }

        public void Add<T>(Action<T> serviceCallback) where T : class, IService
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
                k_Callbacks[type] = result += newAction;
            else
                k_Callbacks[type] = newAction;
            k_RegisteredCallbacks.Add(serviceCallback, newAction);
        }

        public void Remove<T>(Action<T> serviceCallback) where T : class, IService
        {
            if (!k_RegisteredCallbacks.TryGetValue(serviceCallback, out var action))
                return;

            var type = typeof(T);
            if (k_Callbacks.TryGetValue(type, out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    k_Callbacks.Remove(type);
                else
                    k_Callbacks[type] = tempAction;
            }
            k_RegisteredCallbacks.Remove(serviceCallback);
        }
    }
}