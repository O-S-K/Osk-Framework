using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

namespace OSK
{
public class ScriptableObjectManager
{
    public static List<ScriptableID> scriptableIDs = new List<ScriptableID>();
 
    // public ScriptableObjectManager()
    // {
    //     var listSoFormRes = Resources.LoadAll<ScriptableID>("");
    //     foreach (var scriptableID in listSoFormRes)
    //     {
    //         scriptableIDs.Add(scriptableID);
    //     }
    // }  
    
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadScriptableIDs()
    {
        var listSoFormRes = Resources.LoadAll<ScriptableID>("");
        foreach (var scriptableID in listSoFormRes)
        {
            scriptableIDs.Add(scriptableID);
        }
    }
    
    public int Count => scriptableIDs.Count;
    
    public T Get<T>() where T : ScriptableID
    {
        foreach (var scriptableID in scriptableIDs)
        {
            if (scriptableID is T)
                return (T)scriptableID;
        }
        return null;
    }
}
}