#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(ServiceLocatorManager))]
    public class ServiceLocatorEditor : Editor
    {
        private ServiceLocatorManager _serviceLocatorManager;

        private void OnEnable()
        {
            _serviceLocatorManager = (ServiceLocatorManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Registered Services", EditorStyles.boldLabel);
            DisplayRegisteredServices();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Registered Service Listeners", EditorStyles.boldLabel);
            DisplayServiceListeners();
        }

        private void DisplayRegisteredServices()
        {
            // Using reflection to access the private field "k_Services"
            var servicesField = typeof(ServiceLocatorManager).GetField("k_Services", BindingFlags.NonPublic | BindingFlags.Instance);
            if (servicesField != null)
            {
                var services = servicesField.GetValue(_serviceLocatorManager) as Dictionary<Type, IService>;

                if (services != null && services.Count > 0)
                {
                    EditorGUILayout.LabelField($"Service Count: {services.Count}", EditorStyles.boldLabel);
                    foreach (var service in services)
                    {
                        EditorGUILayout.LabelField($"- {service.Key.Name}", EditorStyles.label);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No registered services", EditorStyles.label);
                }
            }
        }

        private void DisplayServiceListeners()
        {
            // Using reflection to access the private field "k_Callbacks"
            var callbacksField = typeof(ServiceLocatorManager).GetField("k_Callbacks", BindingFlags.NonPublic | BindingFlags.Instance);
            if (callbacksField != null)
            {
                var callbacks = callbacksField.GetValue(_serviceLocatorManager) as Dictionary<Type, Action<IService>>;

                if (callbacks != null && callbacks.Count > 0)
                {
                    EditorGUILayout.LabelField($"Listener Count: {callbacks.Count}", EditorStyles.boldLabel);
                    foreach (var callback in callbacks)
                    {
                        EditorGUILayout.LabelField($"- {callback.Key.Name}", EditorStyles.label);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No registered listeners", EditorStyles.label);
                }
            }
        }
    }
}
#endif
