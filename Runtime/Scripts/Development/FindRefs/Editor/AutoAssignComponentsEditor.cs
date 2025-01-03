#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace OSK
{
    [InitializeOnLoad]
    public static class AutoAssignComponentsEditor
    {
        public static bool IsAutoAssignEnabled = true;

        static AutoAssignComponentsEditor()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorApplication.projectChanged += OnProjectChanged;
        }

        private static void OnHierarchyChanged()
        {
            if (IsAutoAssignEnabled)
                AutoAssignForAllMonoBehaviours();
        }

        private static void OnProjectChanged()
        {
            if (IsAutoAssignEnabled)
                AutoAssignForAllMonoBehaviours();
        }
        
        [MenuItem("OSK-Framework/FindRefs/Update References")]
        public static void UpdateReferences()
        {
            AutoAssignForAllMonoBehaviours();
            Debug.Log("All references updated");
        }
        
        [MenuItem("OSK-Framework/FindRefs/Enable Auto-Assign")]
        public static void OnOffAutoAssign()
        {
            IsAutoAssignEnabled = !IsAutoAssignEnabled;
            Debug.Log($"Auto-Assign is {IsAutoAssignEnabled}");
        }

        private static void AutoAssignForAllMonoBehaviours()
        {
            var allGameObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allGameObjects)
            {
                if (PrefabUtility.GetPrefabInstanceHandle(go) != null)
                {
                    // fix prefab in scene
                    var monoBehaviours = go.GetComponentsInChildren<Component>(true);
                    foreach (var mono in monoBehaviours)
                    {
                        if (mono != null)
                        {
                            //Debug.Log($"Auto-assigning for {mono.name} in Prefab Instance {go.name}");
                            ComponentFinder.AutoAssignComponents(mono);
                        }
                    }
                }
                else
                {
                    // fix gameobject in scene
                    var monoBehaviours = go.GetComponentsInChildren<Component>(true);
                    foreach (var mono in monoBehaviours)
                    {
                        if (mono != null)
                        {
                            //Debug.Log($"Auto-assigning for {mono.name} in GameObject {go.name}");
                            ComponentFinder.AutoAssignComponents(mono);
                        }
                    }
                }
            }

            // Fix Prefab in Editor
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                var prefabRoot = prefabStage.prefabContentsRoot;

                var monoBehaviours = prefabRoot.GetComponentsInChildren<Component>(true);
                foreach (var mono in monoBehaviours)
                {
                    if (mono != null)
                    {
                        //Debug.Log($"Auto-assigning for {mono.name} in Prefab Asset {prefabRoot.name}");
                        ComponentFinder.AutoAssignComponents(mono);
                    }
                }
            }

           // if (!Application.isPlaying)
           //     EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}
#endif