using UnityEngine;

public class ScriptableID : ScriptableObject
{
    ScriptableObjectManager scriptableObjectManager = new ScriptableObjectManager();
    
    private void Start()
    {
        scriptableObjectManager.Register(this);
    }
    
    private void OnDestroy()
    {
        scriptableObjectManager.Unregister(this);
    }
}
