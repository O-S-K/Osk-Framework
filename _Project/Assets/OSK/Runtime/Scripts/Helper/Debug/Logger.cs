using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Logger : GameFrameworkComponent
    {
        private Dictionary<string, DateTime> _timesDictionary = new();

        public bool isLogEnabled = true;

        public void Log(object log, LogColor color = LogColor.White, int size = 12)
        {
            if (!isLogEnabled)
                return;
            Debug.Log(($"<><> {log}").Color(color).Size(size));
        }

        public void LogWarning(string log)
        {
            if (!isLogEnabled)
                return;
            Debug.LogWarning($"<><> Warning!\t{log}");
        }

        public void LogError(string log)
        {
            if (!isLogEnabled)
                return;
            Debug.LogError($"<><> Error!\t{log}");
        }

        public void SetTime(string name)
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

        public void ShowTimeDifference(string name)
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