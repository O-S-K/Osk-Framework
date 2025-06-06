using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Logg
    {
        private static bool _isLogEnabled = true; 
        public static void SetLogEnabled(bool value) => _isLogEnabled = value;

        // Test time
        public static void StartTest(PerforInfo info) => info.StartTest(info.label);
        public static void StopTest(PerforInfo info) => info.StopTest();

        // Log
        public static void Log(object log, Color color = default, bool isLog = true)
        {
            if (!_isLogEnabled)
                return;

            if (isLog)
                Debug.Log($"[OSK] {log}".Color(color));
        } 

        // Log warning
        public static void LogWarning(string log,bool isLog = true)
        {
            if (!_isLogEnabled)
                return;
            if (isLog)
                Debug.Log($"[OSK] {log}".Color(Color.yellow));
        } 

        // Log format
        public static void LogFormat(string format,bool isLog = true, params object[] args)
        {
            if (!_isLogEnabled)
                return;
            if (isLog)
                Debug.Log($"[OSK] {string.Format(format, args)}".Color(Color.green));
        }

        // Log error
        public static void LogError(string log,bool isLog = true)
        {
            if (!_isLogEnabled)
                return;
            if (isLog)
                Debug.Log($"[OSK] Error {log}".Color(Color.red));
        } 

        // Log exception
        public static void LogException(Exception ex,bool isLog = true)
        {
            if (!_isLogEnabled)
                return;
            if (isLog)
                Debug.Log($"[OSK] Exception {ex.Message}".Color(Color.red));
        } 

        // Log object
        public static void LogSerializeObject(object obj,bool isLog = true)
        {
            if (!_isLogEnabled)
                return;
            if (isLog)
                Debug.Log($"[OSK] " + Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        } 
        
        // Log format time
        public static void LogFormatTime(string format,bool isLog = true, params object[] args)
        {
            if (!_isLogEnabled)
                return;
            if (isLog)
                Debug.Log($"[OSK] {string.Format(format, args)}".Color(Color.green));
        }

        public static void CheckNullRef(bool isNull, string name,bool isLog = true)
        {
            if (!_isLogEnabled)
                return;

            if (isNull && isLog)
            {
                LogError($"Null Reference: {name}");
            }
        } 
    }

    public static class ExLog
    {
        public static string Bold(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return $"<b>{str}</b>";
        }

        public static string Color(this string text, Color color)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            if (color == default)
                color = UnityEngine.Color.white;

            return text.GetColorHtml(color);
        }

        public static string Size(this string text, int size)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return $"<size={size}>{text}</size>";
        }

        public static string Italic(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return $"<i>{str}</i>";
        }

        public static string Time(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return $"<time>{str}</time>";
        }
    }
}