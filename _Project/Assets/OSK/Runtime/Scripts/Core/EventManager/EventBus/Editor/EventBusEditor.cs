using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(EventBus))]
    public class EventBusEditor : Editor
    {
        private EventBus eventBus;

        private void OnEnable()
        {
            eventBus = (EventBus)target;
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
            var eventField = typeof(EventBus).GetField("_subscribers", BindingFlags.NonPublic | BindingFlags.Instance);
            if (eventField != null)
            {
                var subscribers = eventField.GetValue(eventBus) as Dictionary<Type, List<Action<GameEvent>>>;

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