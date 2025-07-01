using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace OSK
{
    public static class SingletonRegistry
    {
        private static readonly Dictionary<Type, MonoBehaviour> k_Instances = new Dictionary<Type, MonoBehaviour>();

        public static bool AutoInitializeOnStartup = false; // Default is false, set to true to auto-initialize on startup
        private static bool _initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (_initialized || !AutoInitializeOnStartup) return;

            LookUpAttributes(); // Scan for GlobalSingleton and SceneSingleton attributes
            SceneManager.activeSceneChanged += (_, _) => LookUpAttributes(); // Optional auto re-scan
            Application.quitting += OnQuit;

            _initialized = true;
            Logg.Log("[SingletonBootstrapper] Initialized automatically");
        }
        
        private static void LookUpAttributes()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(MonoBehaviour).IsAssignableFrom(t));

            foreach (var type in allTypes)
            {
                var globalAttr = type.GetCustomAttribute<GlobalSingletonAttribute>();
                if (globalAttr != null && globalAttr.AutoInitialize)
                {
                    Logg.Log($"[SingletonBootstrapper] Auto-initializing GlobalSingleton: {type.Name}");
                    SingletonRegistry.Get(type); 
                    continue;
                }

                var sceneAttr = type.GetCustomAttribute<SceneSingletonAttribute>();
                if (sceneAttr != null && sceneAttr.AutoInitialize)
                {
                    string currentScene = SceneManager.GetActiveScene().name;
                    if (sceneAttr.AllowedScenes == null || sceneAttr.AllowedScenes.Length == 0 ||
                        sceneAttr.AllowedScenes.Contains(currentScene))
                    {
                        Logg.Log($"[SingletonBootstrapper] Auto-initializing SceneSingleton: {type.Name}");
                        SingletonRegistry.Get(type);
                    }
                }
            }
        }

        private static void Get(Type type)
        {
            if (!typeof(MonoBehaviour).IsAssignableFrom(type))
            {
                Logg.LogError($"[SingletonRegistry] Type {type.Name} is not a MonoBehaviour");
                return;
            }

            if (k_Instances.TryGetValue(type, out var inst)) return;

            var found = UnityEngine.Object.FindObjectOfType(type);
            if (found != null)
            {
                Register((MonoBehaviour)found);
                return;
            }
            var go = new GameObject(type.Name);
            var created = go.AddComponent(type) as MonoBehaviour;
            if (created == null)
            {
                Logg.LogError($"[SingletonRegistry] Failed to create instance of {type.Name}");
                return;
            }
            Register(created);
        }

        private static void OnQuit()
        {
            Logg.Log("[SingletonBootstrapper] Application quitting");
            _initialized = false;
        }
        public static T RegisterOrGet<T>() where T : MonoBehaviour
        {
            Type type = typeof(T);

            if (k_Instances.TryGetValue(type, out var inst))
                return (T)inst;

            var found = UnityEngine.Object.FindObjectOfType<T>();
            if (found != null)
            {
                Register(found);
                return found;
            }

            var go = new GameObject(type.Name);
            var created = go.AddComponent<T>();
            Register(created);
            return created;
        }

        private static void Register<T>(T instance) where T : MonoBehaviour
        {
            var type = typeof(T);

            if (k_Instances.ContainsKey(type))
            {
                Logg.LogWarning(
                    $"[SingletonRegistry] Duplicate singleton of type {type.Name} detected. Old instance will be overwritten.");
            }

            k_Instances[type] = instance;

            if (type.GetCustomAttribute<GlobalSingletonAttribute>() is { } globalAttr)
            {
                if (globalAttr.IsDontDestroyOnLoad)
                {
                    UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
                }

                Debug.Log($"[SingletonRegistry] Registered GlobalSingleton: {type.Name} | AutoInit: {globalAttr.AutoInitialize} | DontDestroy: {globalAttr.IsDontDestroyOnLoad}");
            }
            else if (type.GetCustomAttribute<SceneSingletonAttribute>() is SceneSingletonAttribute sceneAttr)
            {
                string currentScene = SceneManager.GetActiveScene().name;

                if (sceneAttr.AllowedScenes != null && sceneAttr.AllowedScenes.Length > 0)
                {
                    if (!System.Linq.Enumerable.Contains(sceneAttr.AllowedScenes, currentScene))
                    {
                        Debug.LogError($"[SingletonRegistry] SceneSingleton {type.Name} is not allowed in scene '{currentScene}'. Allowed: {string.Join(", ", sceneAttr.AllowedScenes)}");
                    }
                }

                if (sceneAttr.IsDontDestroyOnLoad)
                {
                    UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
                }

                Debug.Log($"[SingletonRegistry] Registered SceneSingleton: {type.Name} | Scene: {currentScene} | AutoInit: {sceneAttr.AutoInitialize} | DontDestroy: {sceneAttr.IsDontDestroyOnLoad}");
            }

            else
            {
                Debug.Log($"[SingletonRegistry] Registered Singleton: {type.Name} (No specific attribute)");
            }
        }
        
        public static void UnRegister<T>() where T : MonoBehaviour
        {
            var type = typeof(T);
            if (k_Instances.TryGetValue(type, out var instance))
            {
                Logg.Log($"[SingletonRegistry] Unregistered Singleton: {type.Name}");

                if (instance.gameObject)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
                }
                k_Instances.Remove(type);
            }
            else
            {
                Logg.LogWarning($"[SingletonRegistry] No Singleton of type {type.Name} found to unregister.");
            }
        }
        
        public static void Clear<T>() where T : MonoBehaviour
        {
            var type = typeof(T);
            if (k_Instances.ContainsKey(type))
            {
                Logg.Log($"[SingletonRegistry] Removed SceneSingleton: {type.Name}");
                k_Instances.Remove(type);
            }
            else
            {
                Logg.LogWarning($"[SingletonRegistry] No SceneSingleton of type {type.Name} found to remove.");
            }
        }

        public static void ClearAll()
        {
            var removeList = (from kv in k_Instances where kv.Key.GetCustomAttribute<GlobalSingletonAttribute>() == null select kv.Key).ToList();
            foreach (var t in removeList)
            {
                Logg.Log($"[SingletonRegistry] Removed SceneSingleton: {t.Name}");
                k_Instances.Remove(t);
            }
        }
         
        public static IEnumerable<Type> AllTypes() => k_Instances.Keys;
        public static int Count => k_Instances.Count;
    }
}