using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Logg
    {
#if UNITY_EDITOR
        private static readonly bool IsLogEnabled = true;
#else
        private static readonly bool IsLogEnabled = false;
#endif

        // Test time
        public static void StartTest(PerforInfo info) => info.StartTest(info.label);
        public static void StopTest(PerforInfo info) => info.StopTest();

        // Log
        public static void Log(object log, Color color = default, int size = 12)
        {
            if (!IsLogEnabled)
                return;

            Debug.Log(($"[OSK] {log}".Color(color).Size(size)));
        }

        // Log warning
        public static void LogWarning(string log)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] Warning {log}".Color(Color.yellow).Size(14)));
        }

        // Log format
        public static void LogFormat(string format, params object[] args)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] {string.Format(format, args)}".Color(Color.green).Size(12)));
        }

        // Log error
        public static void LogError(string log)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] Error {log}".Color(Color.red).Size(14).Bold()));
        }

        // Log exception
        public static void LogException(Exception ex)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] Exception {ex.Message}".Color(Color.red).Size(14).Bold()));
        }

        // Log object
        public static void LogObject(object obj)
        {
            if (!IsLogEnabled)
                return;
#if LogObjectNewtonsoft
            Debug.Log($"[OSK] " + Newtonsoft.Json.JsonConvert.SerializeObject(obj).Color(ColorCustom.Cyan).Size(12));
#endif
        }

        // Log format time
        public static void LogFormatTime(string format, params object[] args)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] {string.Format(format, args)}".Color(Color.green).Size(12)));
        }

        public static void CheckNullRef(bool isNull, string name)
        {
            if (!IsLogEnabled)
                return;

            if (isNull)
            {
                Logg.LogError($"Null Reference: {name}");
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