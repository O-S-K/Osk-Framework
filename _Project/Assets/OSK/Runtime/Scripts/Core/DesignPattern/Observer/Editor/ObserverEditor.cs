using UnityEngine;
using UnityEditor;
using OSK;


[CustomEditor(typeof(Observer))]
public class ObserverEditor : Editor
{
    private Observer observer;

    private void OnEnable()
    {
        observer = (Observer)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Active Observers", EditorStyles.boldLabel);
        DisplayActiveObservers();
    }

    private void DisplayActiveObservers()
    {
        if (observer == null) return;

        foreach (var topic in observer.dictObserver)
        {
            EditorGUILayout.LabelField(topic.Key, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (var callback in topic.Value)
            {
                // Retrieve the instance of the object that registered the callback
                var targetObject = callback.Target;

                // Display the method name and the name of the script
                if (targetObject != null)
                {
                    string scriptName = targetObject.GetType().Name;
                    EditorGUILayout.LabelField($"- {callback.Method.Name} (from {scriptName})", EditorStyles.label);
                }
                else
                {
                    EditorGUILayout.LabelField($"- {callback.Method.Name} (target is null)", EditorStyles.label);
                }
            }

            EditorGUI.indentLevel--;
        }
    }
}
