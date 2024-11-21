using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace OSK
{
    public  class DIContainer
    {
        [ShowInInspector, ReadOnly]
        private static Dictionary<Type, Func<object>> bindings = new Dictionary<Type, Func<object>>();

        public static void Bind<TInterface, TImplementation>() where TImplementation : TInterface, new()
        {
            bindings[typeof(TInterface)] = () => new TImplementation();
        }

        public static void BindAndProvide<TInterface>(Func<object> provider)
        {
            bindings[typeof(TInterface)] = provider;
        }

        public static TInterface Resolve<TInterface>()
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

        public static void Inject(object target)
        {
            var targetType = target.GetType();
            var fields = targetType.GetFields(System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Public |
                                              System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (bindings.ContainsKey(field.FieldType))
                {
                    var value = bindings[field.FieldType]();
                    field.SetValue(target, value);
                }
            }
        }
    }
}