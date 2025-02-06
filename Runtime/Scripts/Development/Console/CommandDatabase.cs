using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OSK
{
    internal static class CommandDatabase
    {
        public static IReadOnlyDictionary<string, MethodInfo> Registered => methodInfoCache;

        private static Dictionary<string, MethodInfo> methodInfoCache;
        private static int indexCommand;

        public static void ExecuteCommand (string methodName, params string[] args)
        {
            if (methodInfoCache == null || !methodInfoCache.ContainsKey(methodName))
            {
                Debug.LogWarning($"UnityConsole: Command `{methodName}` is not registered in the database.");
                return;
            }
    
            var methodInfo = methodInfoCache[methodName];
            var parametersInfo = methodInfo.GetParameters();
            if (parametersInfo.Length != args.Length)
            {
                Debug.LogWarning($"UnityConsole: Command `{methodName}` requires {parametersInfo.Length} args, while {args.Length} were provided.");
                return;
            }
    
            var parameters = new object[parametersInfo.Length];
            for (int i = 0; i < args.Length; i++)
            {
                parameters[i] = Convert.ChangeType(args[i], parametersInfo[i].ParameterType, System.Globalization.CultureInfo.InvariantCulture);
            }
    
            if (methodInfo.IsStatic)
            {
                methodInfo.Invoke(null, parameters); 
            }
            else
            {
                var instance = Activator.CreateInstance(methodInfo.DeclaringType);
                methodInfo.Invoke(instance, parameters);
            }
        }

        internal static void RegisterCommands (Dictionary<string, MethodInfo> commands = null)
        {
            methodInfoCache = commands ?? AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                .Where(method => method.GetCustomAttribute<ConsoleCommandAttribute>() != null)
                .ToDictionary(method => method.GetCustomAttribute<ConsoleCommandAttribute>().Alias ?? method.Name, StringComparer.OrdinalIgnoreCase);
            
            indexCommand  = 0;
        }

        public static string NextCommand ()
        {
            methodInfoCache.ElementAt(indexCommand);
            indexCommand = indexCommand + 1 < methodInfoCache.Count ? indexCommand + 1 : 0;
            return methodInfoCache.ElementAt(indexCommand).Key;
        }
        
        public static string PreviousCommand ()
        {
            methodInfoCache.ElementAt(indexCommand);
            indexCommand = indexCommand - 1 >= 0 ? indexCommand - 1 : methodInfoCache.Count - 1;
            return methodInfoCache.ElementAt(indexCommand).Key;
        }

        public static void RemoveCommand (string commandName)
        {
            if (methodInfoCache == null || !methodInfoCache.ContainsKey(commandName))
            {
                Debug.LogWarning($"UnityConsole: Command `{commandName}` is not registered in the database.");
                return;
            }
    
            methodInfoCache.Remove(commandName);
        }
    }
}
