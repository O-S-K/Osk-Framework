using System;
using System.Collections.Generic;
using CustomInspector;
using Unity.VisualScripting;
using UnityEngine;

public class DIContainer : MonoBehaviour
{
    [ShowInInspector]
    private  Dictionary<Type, Func<object>> bindings = new Dictionary<Type, Func<object>>();

    public  void Bind<TInterface, TImplementation>() where TImplementation : TInterface, new()
    {
        bindings[typeof(TInterface)] = () => new TImplementation();
    }

    public  void BindAndProvide<TInterface>(Func<object> provider)
    {
        bindings[typeof(TInterface)] = provider;
    }

    public  TInterface Resolve<TInterface>()
    {
        if (bindings.ContainsKey(typeof(TInterface)))
        {
            var instanceProvider = bindings[typeof(TInterface)];
            if (instanceProvider != null)
            {
                return (TInterface)instanceProvider();
            }
            else
            {
                throw new Exception($"No binding found for {typeof(TInterface)}");
            }
        }
        else
        {
            throw new Exception($"No binding found for {typeof(TInterface)}");
        }
    }

    public  void Inject(object target)
    {
        var targetType = target.GetType();
        var fields = targetType.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (bindings.ContainsKey(field.FieldType))
            {
                var value = bindings [field.FieldType]();
                field.SetValue(target, value);
            }
        }
    }
}