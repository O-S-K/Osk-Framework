using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class AutoAssignComponentsEditor
{
    static AutoAssignComponentsEditor()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        EditorApplication.projectChanged += OnProjectChanged;
    }

    private static void OnHierarchyChanged()
    {
        Debug.Log("Hierarchy Changed!");
        AutoAssignForAllMonoBehaviours();
    }

    private static void OnProjectChanged()
    {
        Debug.Log("Project Changed!");
        AutoAssignForAllMonoBehaviours();
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
    }
}
