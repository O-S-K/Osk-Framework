using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

public class BacktraceReferenceInScene : EditorWindow
{
    /// <summary> The result </summary>
    public static List<Component> ReferencingSelection = new List<Component>();
    /// <summary> allComponents in the scene that will be searched to see if they contain the reference </summary>
    private static Component[] allComponents;
    /// <summary> Selection of gameobjects the user made </summary>
    private static GameObject[] selections;
    /// <summary>
    /// Adds context menu to hierarchy window https://answers.unity.com/questions/22947/adding-to-the-context-menu-of-the-hierarchy-tab.html
    /// </summary>
    [MenuItem("GameObject/Find Objects Referencing This", false, 48)]
    public static void InitHierarchy()
    {
        selections = Selection.gameObjects;
        BacktraceSelection(selections);
        GetWindow(typeof(BacktraceReferenceInScene));
    }
    /// <summary>
    /// Display referenced by components in window
    /// </summary>
    public void OnGUI()
    {
        if (selections == null || selections.Length < 1)
        {
            GUILayout.Label("Select source object/s from scene Hierarchy panel.");
            return;
        }
        // display reference that is being checked
        GUILayout.Label(string.Join(", ", selections.Where(go => go != null).Select(go => go.name).ToArray()));
        // handle no references
        if (ReferencingSelection == null || ReferencingSelection.Count == 0)
        {
            GUILayout.Label("is not referenced by any gameobjects in the scene");
            return;
        }
        // display list of references using their component name as the label
        foreach (var item in ReferencingSelection)
        {
            EditorGUILayout.ObjectField(item.GetType().ToString(), item, typeof(GameObject), allowSceneObjects: true);
        }
    }

    // This script finds all objects in scene
    private static Component[] GetAllActiveInScene()
    {
        // Use new version of Resources.FindObjectsOfTypeAll(typeof(Component)) as per https://forum.unity.com/threads/editorscript-how-to-get-all-gameobjects-in-scene.224524/
        var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        List<Component> result = new List<Component>();
        foreach (var rootObject in rootObjects)
        {
            result.AddRange(rootObject.GetComponentsInChildren<Component>());
        }
        return result.ToArray();
    }

    private static void BacktraceSelection(GameObject[] selections)
    {
        if (selections == null || selections.Length < 1)
        {
            return;
        }

        allComponents = GetAllActiveInScene();

        if (allComponents == null)
        {
            return;
        }

        ReferencingSelection.Clear();

        foreach (GameObject selection in selections)
        {
            foreach (Component cOfSelection in selection.GetComponents(typeof(Component)))
            {
                FindObjectsReferencing(cOfSelection);
            }
        }
    }

    private static void FindObjectsReferencing<T>(T cOfSelection) where T : Component
    {
        foreach (Component sceneComponent in allComponents)
        {
            componentReferences(sceneComponent, cOfSelection);
        }
    }

    /// <summary>
    /// Determines if the component makes any references to the second "references" component in any of its inspector fields
    /// </summary>
    private static void componentReferences(Component component, Component references)
    {
        // find all fields exposed in the editor as per https://answers.unity.com/questions/1333022/how-to-get-every-public-variables-from-a-script-in.html
        SerializedObject serObj = new SerializedObject(component);
        SerializedProperty prop = serObj.GetIterator();

        while (prop.NextVisible(true))
        {
            bool isObjectField = prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue != null;
            if (isObjectField && prop.objectReferenceValue == references)
            {
                ReferencingSelection.Add(component);
            }
        }
    }
}
#endif