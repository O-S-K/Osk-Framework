using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        var eventField = typeof(EventBus).GetField("_subscribers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (eventField != null)
        {
            var subscribers = eventField.GetValue(eventBus) as Dictionary<System.Type, List<Action<GameEvent>>>;

            if (subscribers != null && subscribers.Count > 0)
            {
                foreach (var subscriber in subscribers)
                {
                    EditorGUILayout.LabelField($"Event Type: {subscriber.Key.Name}", EditorStyles.boldLabel);
                    foreach (var callback in subscriber.Value)
                    {
                        EditorGUILayout.LabelField($"- Callback: {callback.Method.Name}", EditorStyles.label);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No registered listeners", EditorStyles.label);
            }
        }
    }
}