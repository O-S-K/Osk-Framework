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
        public static void Log(object log, ColorCustom color = default, int size = 12)
        {
            if (!isLogEnabled)
                return;

            Debug.Log(($"[OSK] {log}").Color(color).Size(size));
        }

        // Log warning
        public static void LogWarning(string log)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] Warning {log}").Color(ColorCustom.Yellow).Size(14));
        }

        // Log format
        public static void LogFormat(string format, params object[] args)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] {string.Format(format, args)}").Color(ColorCustom.Green).Size(12));
        }

        // Log error
        public static void LogError(string log)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] Error {log}").Color(ColorCustom.Red).Size(14).Bold());
        }

        // Log exception
        public static void LogException(Exception ex)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"[OSK] Exception {ex.Message}").Color(ColorCustom.Red).Size(14).Bold());
        }

        // Set time for a task
        public static void SetTime(string name)
        {
            if (!isLogEnabled)
                return;
            Log($"Task starts: {name}", ColorCustom.Cyan);
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

            Log($"Task finished: {name} - Time {(DateTime.Now - _timesDictionary[name]).TotalSeconds}", ColorCustom.Cyan);
        }
    }

    public static class ExLog
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";

        public static string Color(this string str, ColorCustom clr) =>  str.GetColorHTML(clr);
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => $"<size={size}>{str}</size>";
    }
}