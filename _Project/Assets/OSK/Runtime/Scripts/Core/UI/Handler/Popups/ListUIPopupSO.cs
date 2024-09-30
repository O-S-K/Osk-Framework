using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CustomInspector;
using OSK;
using UnityEngine;

[CreateAssetMenu(fileName = "ListUIPopupSO", menuName = "OSK/UI/ListUIPopupSO")]
public class ListUIPopupSO : ScriptableObject
{
    [TableList, SerializeField] private List<Popup> _popups = new List<Popup>();
    public List<Popup> Popups => _popups;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // check unique ui screen  type
        var popupTypes = new List<Type>();
        foreach (var popup in _popups)
        {
            if (popupTypes.Contains(popup.GetType()))
            {
                Debug.LogError($"Popup Type {popup.GetType()} exists in the list. Please remove it.");
            }
            else
            {
                popupTypes.Add(popup.GetType());
            }
        }
    }
    
    [Button("Add All Popup Form Resources")]
    public void AddAllPopupFormResources()
    {
        _popups.Clear();
        var popupTypes = new List<Type>();
        var popupTypesInResources = Resources.LoadAll<Popup>("").ToList();
        foreach (var popup in popupTypesInResources)
        {
            if (popupTypes.Contains(popup.GetType()))
            {
                Debug.LogError($"Popup Type {popup.GetType()} exists in the list. Please remove it.");
            }
            else
            {
                popupTypes.Add(popup.GetType());
                _popups.Add(popup);
            }
        }
    }
    
    #endif
    
}