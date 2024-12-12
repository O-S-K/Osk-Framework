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

        [SerializeReference] private static Dictionary<Type, object> k_Registry = new();

        public static void InstallBindAndInjects()
        {
            // Find all modules implementing IDependencyProvider and register the dependencies they provide
            var monoBehaviours = FindMonoBehaviours();
            var providers = monoBehaviours.OfType<IProvider>();

            foreach (var provider in providers)
            {
                Bind(provider);
            }

            // Find all injectable objects and inject their dependencies
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
            }

            if (instance is not IProvider)
            {
                Logg.LogError("Instance does not contain the required component has IProvider.");
            }
        }

        public static void BindFromPrefab(GameObject mono)
        {
            if (mono == null)
            {
                Logg.LogError("Prefab cannot be null.");
            }

            var instance = Object.Instantiate(mono).GetComponent<IProvider>();
            if (instance == null)
            {
                Logg.LogError("Prefab does not contain the required component has IProvider.");
            }
            else
            {
                Logg.Log($"[BindFromPrefab]: {mono.GetType().Name} ->>> '{instance.GetType().Name}'.", Color.green);
            }
        }

        public static T BindFromPrefab<T>(T prefab) where T : MonoBehaviour, IProvider
        {
            if (prefab == null)
            {
                Logg.LogError("Prefab cannot be null.");
            }

            var instance = Object.Instantiate(prefab).GetComponent<T>();
            if (instance == null)
            {
                Logg.LogError("Prefab does not contain the required component has IProvider.");
            }

            return instance;
        }

        public static T BindAsSingle<T>(T instance) where T : MonoBehaviour, IProvider
        {
            if (instance == null)
            {
                Logg.LogError("Instance cannot be null.");
            }

            if (instance is not IProvider provider)
                return null;

            Bind(provider);
            Inject(instance);
            return instance;
        }

        public static void Inject(object instance)
        {
            var type = instance.GetType();
            var injectableFields = type.GetFields(k_BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);

                if (resolvedInstance == null)
                {
                    Logg.LogError(
                        $"[Injecting] Dependency '{fieldType.Name}' not bind and injected into '{type.Name}.{injectableField.Name}'.");
                }
                else
                {
                    Logg.Log($"[Injecting]: {fieldType.Name} ->>> {type.Name}.{injectableField.Name}", Color.green);
                    injectableField.SetValue(instance, resolvedInstance);
                }
            }

            // Inject into methods
            var injectableMethods = type.GetMethods(k_BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    Logg.LogError(
                        $"Failed to inject dependencies into method '{injectableMethod.Name}' of class '{type.Name}'.");
                }

                injectableMethod.Invoke(instance, resolvedInstances);
            }

            // Inject into properties
            var injectableProperties = type.GetProperties(k_BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableProperty in injectableProperties)
            {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = Resolve(propertyType);
                if (resolvedInstance == null)
                {
                    Logg.LogError(
                        $"Failed to inject dependency into property '{injectableProperty.Name}' of class '{type.Name}'.");
                }

                injectableProperty.SetValue(instance, resolvedInstance);
            }
        }

        private static void Bind(IProvider provider)
        {
            var methods = provider.GetType().GetMethods(k_BindingFlags);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute)))
                    continue;

                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    Logg.Log($"[Bind]: {provider.GetType().Name} ->>> '{returnType.Name}'.", Color.green);
                    k_Registry.Add(returnType, providedInstance);
                }
                else
                {
                    Logg.LogError(
                        $"[Bind] method '{method.Name}' in class '{provider.GetType().Name}' returned null when providing type '{returnType.Name}'.");
                }
            }
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

        public static Dictionary<Type, object> GetRegisteredDependencies()
        {
            return new Dictionary<Type, object>(k_Registry);
        }

        public static bool IsDependencyRegistered<T>()
        {
            return k_Registry.ContainsKey(typeof(T));
        }

        public static void ClearDependencies()
        {
            foreach (var monoBehaviour in FindMonoBehaviours())
            {
                var type = monoBehaviour.GetType();
                var injectableFields = type.GetFields(k_BindingFlags)
                    .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

                foreach (var injectableField in injectableFields)
                {
                    injectableField.SetValue(monoBehaviour, null);
                }
            }

            Logg.Log("[Injector] All injectable fields cleared.", Color.green);
        }

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private static object Resolve(Type type)
        {
            if (k_Registry.TryGetValue(type, out var instance))
            {
                return instance;
            }

            Logg.LogError(
                $"[Resolve] Dependency '{type.Name}' not found in registry. Please add IProvider to the class.");
            return null;
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