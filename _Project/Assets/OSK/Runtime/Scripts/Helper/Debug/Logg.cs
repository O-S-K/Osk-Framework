using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Logg
    {
        private static Dictionary<string, DateTime> _timesDictionary = new();
#if UNITY_EDITOR
        public static bool isLogEnabled = true;
#else
        public static bool isLogEnabled = false;
#endif
        // Log
        public static void Log(object log, LogColor color = LogColor.White, int size = 12)
        {
            if (!isLogEnabled)
                return;
            
            Debug.Log(($"[OSK] {log}").Color(color).Size(size)); 
        }

        // Log pass
        public static void LogPass(object log, bool isPass)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] {log}").Color(isPass ? LogColor.Green : LogColor.Black));
        }

        // Log warning
        public static void LogWarning(string log)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] Warning {log}").Color(LogColor.Yellow).Size(14));
        }

        // Log error
        public static void LogError(string log)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] Error {log}").Color(LogColor.Red).Size(14).Bold());
        }

        // Log exception
        public static void LogException(Exception ex)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] Exception {ex.Message}").Color(LogColor.Red).Size(14).Bold());
        }

        // Set time for a task
        public static void SetTime(string name)
        {
            if (!isLogEnabled)
                return;
            Log($"Task starts: {name}", LogColor.Orange);
            if (_timesDictionary.ContainsKey(name))
            {
                _timesDictionary[name] = DateTime.Now;
                return;
            }

            _timesDictionary.Add(name, DateTime.Now);
        }

        // Show time difference between SetTime and ShowTimeDifference
        public static void ShowTimeDifference(string name)
        {
            if (!isLogEnabled)
                return;
            if (!_timesDictionary.ContainsKey(name))
            {
                LogError($"{name} is not set before!");
                return;
            }

            Log($"Task finished: {name} - Time {(DateTime.Now - _timesDictionary[name]).TotalSeconds}", LogColor.Green);
        }
    }

    public static class ExLog
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Color(this string str, LogColor clr) => $"<color={clr}>{str}</color>";
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => $"<size={size}>{str}</size>";
    }

    public enum LogColor
    {
        Red,
        Yellow,
        Blue,
        Orange,
        Black,
        White,
        Green
    }
}