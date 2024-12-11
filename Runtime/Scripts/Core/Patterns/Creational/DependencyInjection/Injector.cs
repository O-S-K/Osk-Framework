using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OSK
{
    [DefaultExecutionOrder(-1000)]
    public class Injector : MonoBehaviour
    {
        [SerializeReference]
        private const BindingFlags k_BindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [SerializeReference] private readonly Dictionary<Type, object> k_Registry = new();

        private void Awake()
        {
            var monoBehaviours = FindMonoBehaviours();
            // Find all modules implementing IDependencyProvider and register the dependencies they provide
            var providers = monoBehaviours.OfType<IProvider>();

            foreach (var provider in providers)
            {
                Register(provider);
            }

            // Find all injectable objects and inject their dependencies
            var injectables = monoBehaviours.Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        // Register an instance of a type outside of the normal dependency injection process
        public void Register<T>(T instance)
        {
            k_Registry[typeof(T)] = instance;
        }

        private void Inject(object instance)
        {
            var type = instance.GetType();

            // Inject into fields
            var injectableFields = type.GetFields(k_BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                if (injectableField.GetValue(instance) != null)
                {
                    Debug.LogWarning(
                        $"[Injector] Field '{injectableField.Name}' of class '{type.Name}' is already set.");
                    continue;
                }

                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception(
                        $"Failed to inject dependency into field '{injectableField.Name}' of class '{type.Name}'.");
                }

                injectableField.SetValue(instance, resolvedInstance);
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
                    throw new Exception(
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
                    throw new Exception(
                        $"Failed to inject dependency into property '{injectableProperty.Name}' of class '{type.Name}'.");
                }

                injectableProperty.SetValue(instance, resolvedInstance);
            }
        }

        private void Register(IProvider provider)
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
                    Debug.Log(
                        $"[Injector] Registered provider method '{method.Name}' in class '{provider.GetType().Name}' providing type '{returnType.Name}'.".Color(Color.green));
                    k_Registry.Add(returnType, providedInstance);
                }
                else
                {
                    throw new Exception(
                        $"Provider method '{method.Name}' in class '{provider.GetType().Name}' returned null when providing type '{returnType.Name}'.");
                }
            }
        }

        public void ValidateDependencies()
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
                Debug.Log("[Validation] All dependencies are valid.".Color(Color.green));
            }
            else
            {
                Debug.LogError($"[Validation] {invalidDependencyList.Count} dependencies are invalid:");
                foreach (var invalidDependency in invalidDependencyList)
                {
                    Debug.LogError(invalidDependency);
                }
            }
        }

        private HashSet<Type> GetProvidedDependencies(IEnumerable<IProvider> providers)
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

        public void ClearDependencies()
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

            Debug.Log("[Injector] All injectable fields cleared.");
        }

        private object Resolve(Type type)
        {
            k_Registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        private static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }

        private static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(k_BindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }
    }
}