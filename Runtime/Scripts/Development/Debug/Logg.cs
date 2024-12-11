using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Logg
    {
        private static bool IsLogEnabled  = false;
        
        // Test time
        public static void StartTest(PerforInfo info) =>  info.StartTest(info.label);
        public static void StopTest(PerforInfo info) =>  info.StopTest();

        // Log
        public static void Log(object log, Color color = default, int size = 12)
        {
            if (!IsLogEnabled)
                return;

            Debug.Log(($"[OSK] {log}").Color(color).Size(size));
        }

        // Log warning
        public static void LogWarning(string log)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] Warning {log}").Color(Color.yellow).Size(14));
        }

        // Log format
        public static void LogFormat(string format, params object[] args)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] {string.Format(format, args)}").Color(Color.green).Size(12));
        }

        // Log error
        public static void LogError(string log)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] Error {log}").Color(Color.red).Size(14).Bold());
        }

        // Log exception
        public static void LogException(Exception ex)
        {
            if (!IsLogEnabled)
                return;
            Debug.Log(($"[OSK] Exception {ex.Message}").Color(Color.red).Size(14).Bold());
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
            Debug.Log(($"[OSK] {string.Format(format, args)}").Color(Color.green).Size(12));
        }
        
        public static void CheckNullRef(bool isNull, string name)
        {
            if (isNull)
            {
                Logg.LogError($"Null Reference: {name}");
            }
        }
    }

    public static class ExLog
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";

        public static string Color(this string str, Color clr) => str.GetColorHTML(clr);
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => $"<size={size}>{str}</size>";
        public static string Time(this string str) => $"<time>{DateTime.Now}</time>";
    }
}