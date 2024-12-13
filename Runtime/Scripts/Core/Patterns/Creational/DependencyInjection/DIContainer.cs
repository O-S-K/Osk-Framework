using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OSK
{
    [DefaultExecutionOrder(-999)]
    public static class DIContainer
    {
        [SerializeReference] private const BindingFlags k_BindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static Dictionary<Type, Dictionary<string, object>> k_Registry = new();


        public static void InstallBindAndInjects()
        {
            var monoBehaviours = FindMonoBehaviours();
            var providers = monoBehaviours.OfType<IProvider>();

            foreach (var provider in providers)
            {
                Bind(provider);
            }

            var injectables = monoBehaviours.Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        public static void Bind<T>(T instance)
        {
            if (instance == null)
            {
                Logg.LogError("Instance cannot be null.");
                return;
            }

            if (instance is not IProvider)
            {
                Logg.LogError("Instance does not contain the required component IProvider.");
                return;
            }

            Bind((IProvider)instance);
        }

        public static void BindFromPrefab(GameObject mono)
        {
            if (mono == null)
            {
                Logg.LogError("Prefab cannot be null.");
                return;
            }

            var instance = Object.Instantiate(mono).GetComponent<IProvider>();
            if (instance == null)
            {
                Logg.LogError("Prefab does not contain the required component IProvider.");
            }
        }

        public static T BindFromPrefab<T>(T prefab) where T : MonoBehaviour, IProvider
        {
            if (prefab == null)
            {
                Logg.LogError("Prefab cannot be null.");
                return null;
            }

            var instance = Object.Instantiate(prefab);
            if (instance == null)
            {
                Logg.LogError("Prefab does not contain the required component IProvider.");
                return null;
            }

            return instance;
        }

        public static void Bind(IProvider provider)
        {
            foreach (var method in provider.GetType().GetMethods(k_BindingFlags)
                         .Where(m => Attribute.IsDefined(m, typeof(ProvideAttribute))))
            {
                var provideAttribute = (ProvideAttribute)Attribute.GetCustomAttribute(method, typeof(ProvideAttribute));
                var key = provideAttribute?.Key;
                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);

                if (providedInstance != null)
                {
                    if (!k_Registry.ContainsKey(returnType))
                    {
                        k_Registry[returnType] = new Dictionary<string, object>();
                    }

                    if (key != null && k_Registry[returnType].ContainsKey(key))
                    {
                        Logg.LogWarning(
                            $"[Bind]: Duplicate binding detected for '{returnType.Name}' with key '{key}'. Skipping registration.");
                        continue; // Skip adding this entry to avoid duplicate key error
                    }

                    k_Registry[returnType][key ?? string.Empty] = providedInstance;
                    Logg.Log($"[Bind]: {provider.GetType().Name} -> '{returnType.Name}' with key '{key}'.");
                }
                else
                {
                    Logg.LogError($"[Bind]: {provider.GetType().Name} -> '{returnType.Name}' with key '{key}' failed.");
                }
            }
        }

        public static void UnBind<T>(string key = null)
        {
            var type = typeof(T);
            if (k_Registry.ContainsKey(type))
            {
                if (string.IsNullOrEmpty(key))
                {
                    k_Registry[type].Clear();
                    Logg.Log($"Unbound all instances of {type.Name}", Color.green);
                }
                else if (k_Registry[type].ContainsKey(key))
                {
                    k_Registry[type].Remove(key);
                    Logg.Log($"Unbound {type.Name} with key {key}", Color.green);
                }
                else
                {
                    Logg.LogWarning($"No binding found for {type.Name} with key '{key}'.");
                }
            }
            else
            {
                Logg.LogWarning($"No registry found for {type.Name}.");
            }
        }

        public static void Inject(object instance)
        {
            if (instance == null)
            {
                Logg.LogError("Instance cannot be null.");
                return;
            }

            var type = instance.GetType();

            // Inject fields
            foreach (var field in type.GetFields(k_BindingFlags).Where(f => Attribute.IsDefined(f, typeof(InjectAttribute))))
            {
                var injectAttribute = (InjectAttribute)Attribute.GetCustomAttribute(field, typeof(InjectAttribute));
                var resolvedInstance = Resolve(field.FieldType, injectAttribute.Key);
                if (resolvedInstance == null)
                {
                    Logg.LogError(
                        $"[Injecting] Dependency '{field.FieldType.Name}' with key '{injectAttribute.Key}' not found for '{type.Name}.{field.Name}'.");
                }
                else
                {
                    field.SetValue(instance, resolvedInstance);
                    Logg.Log(
                        $"[Injecting]: {field.FieldType.Name} -> {type.Name}.{field.Name} (Key: {injectAttribute.Key})",
                        Color.green);
                }
            }

            // Inject methods
            foreach (var method in type.GetMethods(k_BindingFlags) .Where(m => Attribute.IsDefined(m, typeof(InjectAttribute))))
            {
                var injectAttribute = (InjectAttribute)Attribute.GetCustomAttribute(method, typeof(InjectAttribute));
                var parameters = method.GetParameters().Select(p => Resolve(p.ParameterType, injectAttribute.Key))
                    .ToArray();
                if (parameters.Any(p => p == null))
                {
                    Logg.LogError($"Failed to inject dependencies into method '{method.Name}' of class '{type.Name}'.");
                }
                else
                {
                    method.Invoke(instance, parameters);
                }
            }

            // Inject properties
            foreach (var property in type.GetProperties(k_BindingFlags)
                         .Where(p => Attribute.IsDefined(p, typeof(InjectAttribute))))
            {
                var injectAttribute = (InjectAttribute)Attribute.GetCustomAttribute(property, typeof(InjectAttribute));
                var resolvedInstance = Resolve(property.PropertyType, injectAttribute.Key);
                if (resolvedInstance == null)
                {
                    Logg.LogError($"Failed to inject dependency into property '{property.Name}' of class '{type.Name}'.");
                }
                else
                {
                    property.SetValue(instance, resolvedInstance);
                }
            }
        }

        public static T Resolve<T>(string key = null)
        {
            return (T)Resolve(typeof(T), key);
        }
        

        private static object Resolve(Type type, string key)
        {
            if (string.IsNullOrEmpty(key))
                key = string.Empty;
            
            if (k_Registry.ContainsKey(type) && k_Registry[type].ContainsKey(key))
            {
                return k_Registry[type][key];
            }

            Logg.LogError($"[Resolve]: {type.Name} with key '{key}' not found.");
            return null;
        }

        public static void ValidateDependencies()
        {
            var monoBehaviours = FindMonoBehaviours();
            var providers = monoBehaviours.OfType<IProvider>();
            var providedDependencies = GetProvidedDependencies(providers);

            var invalidDependencies = monoBehaviours
                .SelectMany(mb => mb.GetType().GetFields(k_BindingFlags), (mb, field) => new { mb, field })
                .Where(t => Attribute.IsDefined(t.field, typeof(InjectAttribute)))
                .Where(t => !providedDependencies.Contains(t.field.FieldType) && t.field.GetValue(t.mb) == null)
                .Select(t =>
                    $"[Validation] {t.mb.GetType().Name} is missing dependency {t.field.FieldType.Name} on GameObject {t.mb.gameObject.name}");

            var invalidDependencyList = invalidDependencies.ToList();

            if (!invalidDependencyList.Any())
            {
                Logg.Log("[Validation] All dependencies are valid.", Color.green);
            }
            else
            {
                Logg.LogError($"[Validation] {invalidDependencyList.Count} dependencies are invalid:");
                foreach (var invalidDependency in invalidDependencyList)
                {
                    Logg.LogError(invalidDependency);
                }
            }
        }

        private static HashSet<Type> GetProvidedDependencies(IEnumerable<IProvider> providers)
        {
            var providedDependencies = new HashSet<Type>();
            foreach (var provider in providers)
            {
                var methods = provider.GetType().GetMethods(k_BindingFlags);

                foreach (var method in methods)
                {
                    if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                    var returnType = method.ReturnType;
                    providedDependencies.Add(returnType);
                }
            }

            return providedDependencies;
        }


        public static Dictionary<Type, List<object>> GetRegisteredDependencies()
        {
            return k_Registry.ToDictionary(k => k.Key, v => v.Value.Values.ToList());
        }

        public static void ClearDependencies()
        {
            k_Registry?.Clear();
        }

        public static void ClearDependencies<T>()
        {
            k_Registry.Remove(typeof(T));
        }

        private static MonoBehaviour[] FindMonoBehaviours()
        {
#if UNITY_2022_1_OR_NEWER
            return Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
#else
            return Object.FindObjectsOfType<MonoBehaviour>();
#endif
        }

        private static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(k_BindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }
    }
}