#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace OSK
{
    public static class SingletonScanner
    {
        [MenuItem("OSK-Framework/Tools/Singleton/Scan GlobalSingleton Classes")]
        public static void ScanGlobalSingletons()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => t.GetCustomAttribute<GlobalSingletonAttribute>() != null);

            Debug.Log("[SingletonScanner] Found GlobalSingleton Classes:");
            foreach (var type in types)
            {
                Debug.Log(" - " + type.FullName);
            }
        }

        [MenuItem("OSK-Framework/Tools/Singleton/Scan SceneSingleton Classes")]
        public static void ScanSceneSingletons()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => t.GetCustomAttribute<SceneSingletonAttribute>() != null);

            Debug.Log("[SingletonScanner] Found SceneSingleton Classes:");
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<SceneSingletonAttribute>();
                string scenes = (attr.AllowedScenes != null && attr.AllowedScenes.Length > 0)
                    ? string.Join(", ", attr.AllowedScenes)
                    : "(All Scenes)";
                Debug.Log($" - {type.FullName} | Allowed Scenes: {scenes}");
            }
        }

        [MenuItem("OSK-Framework/Tools/Singleton/Print Registered Singletons")]
        public static void PrintRegisteredSingletons()
        {
            Debug.Log($"[SingletonRegistry] Total: {SingletonRegistry.Count}");
            foreach (var type in SingletonRegistry.AllTypes())
            {
                Debug.Log($" - {type.Name}");
            }
        }
    }
#endif
}