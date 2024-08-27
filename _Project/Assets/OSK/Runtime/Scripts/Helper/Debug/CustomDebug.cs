using System;
using UnityEngine;

namespace OSK
{
    public static class CustomDebug
    {
        static bool isLog = false;
        public static void Log(object message)
        {
            if (isLog)
            {
                Debug.Log("_Log: " + message);
            }
        }

        public static void Log(object tile, object message)
        {
            if (isLog)
            {
                Debug.Log($"_Log_{tile}: " + message);
            }
        }

        public static void LogWarring(object message)
        {
            if (isLog)
            {
                Debug.LogWarning("_LogWarring: " + message);
            }
        }

        public static void LogError(object message)
        {
            if (isLog)
            {
                Debug.LogError("_LogError: " + message);
            }
        }

        public static void LogException(Exception _exception)
        {
            if (isLog)
            {
                Debug.LogException(_exception);
            }
        }

        public static void PauseEditor()
        {
            if (isLog)
            {
                Debug.Log("_Log: Pause Editor");
                Debug.Break();
            }
        }


    }
}
