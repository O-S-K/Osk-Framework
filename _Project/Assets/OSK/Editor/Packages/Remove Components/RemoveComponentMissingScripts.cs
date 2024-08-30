using System.Linq;
using UnityEditor;
using UnityEngine;

public class RemoveComponentMissingScripts : Editor
{
    [MenuItem("Tools/Remove Missing Scripts In Obect")]
    public static void RemoveMissingScriptsInObect()
    {
        var allObject = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = allObject.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        foreach (var obj in allObject)
        {
            EditorUtility.SetDirty(obj);
        }

        AssetDatabase.Refresh();
        UnityEngine.Debug.Log($"<b><color=#ffa500ff>Removed {count} missing scripts</color></b>");
    }

}
