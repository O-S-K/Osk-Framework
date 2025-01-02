#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(EventBusManager))]
    public class EventBusEditor : Editor
    {
        private EventBusManager _eventBusManager;

        private void OnEnable()
        {
            _eventBusManager = (EventBusManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Registered Event Listeners", EditorStyles.boldLabel);
            DisplayEventListeners();
        }

        private void DisplayEventListeners()
        {
            var eventField = typeof(EventBusManager).GetField("_subscribers", BindingFlags.NonPublic | BindingFlags.Instance);
            if (eventField != null)
            {
                var subscribers = eventField.GetValue(_eventBusManager) as Dictionary<Type, List<Action<GameEvent>>>;

                if (subscribers != null && subscribers.Count > 0)
                {
                    EditorGUILayout.LabelField($"Event List: {subscribers.Count}", EditorStyles.boldLabel);
                    foreach (var subscriber in subscribers)
                    {
                        EditorGUILayout.LabelField($".     {subscriber.Key.Name}", EditorStyles.boldLabel);
                        // foreach (var callback in subscriber.Value)
                        // {
                        //     EditorGUILayout.LabelField($"- Callback: {callback.Method.Name}", EditorStyles.label);
                        // }
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