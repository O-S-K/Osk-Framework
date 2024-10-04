using UnityEngine;
using System.Collections.Generic;
using CustomInspector;

[CreateAssetMenu(fileName = "NamingRules", menuName = "Tools/NamingRules")]
public class NamingRules : ScriptableObject
{
    [TableList]
    public List<NamingRule> rules;
}

[System.Serializable]
public class NamingRule
{
    public ComponentTypeSelector componentType;
    public string prefix;
}

// Custom class to select a component type from a dropdown
[System.Serializable]
public class ComponentTypeSelector
{
    public string selectedComponent;
}