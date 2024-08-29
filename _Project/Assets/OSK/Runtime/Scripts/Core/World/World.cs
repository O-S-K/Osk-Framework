using System;
using System.Collections.Generic;

public partial class World
{
    private static Dictionary<Type, GameFrameworkComponent> _componentRegistry = new Dictionary<Type, GameFrameworkComponent>();

    public static T Get<T>() where T : GameFrameworkComponent
    {
        _componentRegistry.TryGetValue(typeof(T), out var component);
        return component as T;
    }

    public static T[] Gets<T>() where T : GameFrameworkComponent
    {
        var components = new List<T>();

        foreach (var kvp in _componentRegistry)
        {
            if (kvp.Value is T)
            {
                components.Add(kvp.Value as T);
            }
        }

        return components.ToArray();
    }

    public static void Register<T>(T component) where T : GameFrameworkComponent
    {
        var type = typeof(T);
        if (!_componentRegistry.ContainsKey(type))
        {
            _componentRegistry.Add(type, component);
        }
    }

    public static void CleanAll()
    {
        foreach (var component in _componentRegistry.Values)
        {
            if (component != null)
            {
                Destroy(component.gameObject);
            }
        }
        _componentRegistry.Clear();
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
