using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager
{
    public List<ScriptableID> scriptableIDs = new List<ScriptableID>();

    public void Register(ScriptableID scriptableID)
    {
        if (scriptableIDs.Contains(scriptableID))
            return;
        scriptableIDs.Add(scriptableID);
    }
    
    public int Count => scriptableIDs.Count;
    
    public T Get<T>() where T : ScriptableID
    {
        foreach (var scriptableID in scriptableIDs)
        {
            if (scriptableID is T)
            {
                return (T)scriptableID;
            }
        }
        return null;
    }

    public void Unregister(ScriptableID scriptableID)
    {
        if (scriptableIDs.Contains(scriptableID))
        {
            scriptableIDs.Remove(scriptableID);
        }
    }
}