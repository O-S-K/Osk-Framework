#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using OSK;

namespace OSK
{
    [CustomEditor(typeof(ObserverManager))]
    public class ObserverEditor : Editor
    {
        private ObserverManager _observerManager;

        private void OnEnable()
        {
            _observerManager = (ObserverManager)target;
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
            if (_observerManager == null) return;

            foreach (var topic in _observerManager.k_ObserverCallBack)
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
}
#endif