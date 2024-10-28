using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;

namespace Injector
{
    public class Validator : MonoBehaviour
    {
        [InitializeOnLoadMethod]
        [MenuItem("OSK-Framework/Injector/Check")]
        private static void Check()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var attribute = Array.Find(type.GetCustomAttributes(false), x => x.GetType() == typeof(IgnoreOnCheckAttribute));
                    if (attribute != null)
                        continue;

                    try
                    {
                        DescriptorsHolder.GetDescriptor(type, true);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(exception.Message);
                    }
                }
            }
        }
    }
}