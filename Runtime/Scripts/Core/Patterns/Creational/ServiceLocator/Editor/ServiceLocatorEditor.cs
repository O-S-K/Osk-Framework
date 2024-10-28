#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(ServiceLocator))]
    public class ServiceLocatorEditor : Editor
    {
        private ServiceLocator serviceLocator;

        private void OnEnable()
        {
            serviceLocator = (ServiceLocator)target;
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
            var servicesField = typeof(ServiceLocator).GetField("k_Services", BindingFlags.NonPublic | BindingFlags.Instance);
            if (servicesField != null)
            {
                var services = servicesField.GetValue(serviceLocator) as Dictionary<Type, IService>;

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
            var callbacksField = typeof(ServiceLocator).GetField("k_Callbacks", BindingFlags.NonPublic | BindingFlags.Instance);
            if (callbacksField != null)
            {
                var callbacks = callbacksField.GetValue(serviceLocator) as Dictionary<Type, Action<IService>>;

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
