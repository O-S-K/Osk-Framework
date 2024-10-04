using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using OSK;
using UnityEngine;

[CreateAssetMenu(fileName = "ListUIScreenSO", menuName = "OSK/UI/ListUIScreenSO")]
public class ListUIScreenSO : ScriptableID
{
    [TableList, SerializeField] private List<UIScreen> _uiScreens = new List<UIScreen>();
    public List<UIScreen> UIScreens => _uiScreens;


#if UNITY_EDITOR
    private void OnValidate()
    {
        // check unique ui screen  type
        var uiScreenTypes = new List<Type>();
        foreach (var uiScreen in _uiScreens)
        {
            if (uiScreenTypes.Contains(uiScreen.GetType()))
            {
                OSK.Logg.LogError($"UI Screen Type {uiScreen.GetType()} exists in the list. Please remove it.");
            }
            else
            {
                uiScreenTypes.Add(uiScreen.GetType());
            }
        }
    }
    
    [Button("Add All UI Screen Form Resources")]
    public void AddAllUIScreenFormResources()
    {
        _uiScreens.Clear();
        var uiScreenTypes = new List<Type>();
        var uiScreenTypesInResources = Resources.LoadAll<UIScreen>("").ToList();
        foreach (var uiScreen in uiScreenTypesInResources)
        {
            if (uiScreenTypes.Contains(uiScreen.GetType()))
            {
                OSK.Logg.LogError($"UI Screen Type {uiScreen.GetType()} exists in the list. Please remove it.");
            }
            else
            {
                uiScreenTypes.Add(uiScreen.GetType());
                _uiScreens.Add(uiScreen);
            }
        }
    }
#endif
}