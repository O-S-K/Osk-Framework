using UnityEngine;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;

namespace OSK
{
    public static class AutoAssignComponents
    {
        [MenuItem("OSK-Framework/FindRefs/Update All Refs")]
        public static void AutoAssignForAll()
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
            AutoFinds.GetAutoRefs(null);
            Debug.Log("All References Updated");
        }
        
        public static void AutoAssignSelf(Transform go)
        {
            var monoBehaviours = go.GetComponents<MonoBehaviour>(); 
            foreach (var mono in monoBehaviours)
            {
                if (mono != null) ComponentFinder.AutoAssignComponents(mono);
            }
            AutoFinds.GetAutoRefs(go);
        }
    }
}
#endif