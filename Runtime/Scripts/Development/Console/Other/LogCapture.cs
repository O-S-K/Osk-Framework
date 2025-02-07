using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    public class LogCapture : SingletonMono<LogCapture>
    {
        public List<string> logMessages = new List<string>();
        public bool isLogEnabled = false;
        private string logFilePath;
        private int logMaxLine = 25;

        private void Awake()
        {
            if (isLogEnabled)
            {
                Application.logMessageReceived += HandleLog;
            }
        }

        private void OnDestroy()
        {
            if (isLogEnabled)
            {
                Application.logMessageReceived -= HandleLog;
            }
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        { 
            string color = "";
            if(type == LogType.Error || type == LogType.Exception)
            {
                color= "#FF0000"; // Red
            }
            else if(type == LogType.Warning)
            {
                color= "#FFFF00"; // Yellow
            }
            else
            {
                color= "#FFFFFF"; // White
            } 

            string logEntry = $"<color={color}>{System.DateTime.Now} [{type}] {logString}</color>\n";

            if (type == LogType.Error || type == LogType.Exception)
            {
                logEntry += $"<color={color}>StackTrace: {stackTrace}</color>\n";
            }
            if (logMessages.Count >= logMaxLine) logMessages.RemoveAt(0);
            logMessages.Add(logEntry);
        }

        [Button]
        public void ExportLogToFile()
        {
            logFilePath = Path.Combine(Application.persistentDataPath, "LogCapture.txt");
            File.WriteAllLines(logFilePath, logMessages);
            Logg.Log($"Logs exported to: {logFilePath}");
        }

        [Button]
        public void ClearLogs()
        {
            logMessages.Clear();
            logFilePath = Path.Combine(Application.persistentDataPath, "LogCapture.txt");
            File.Delete(logFilePath);
            Logg.Log("Logs cleared.");
        }

        [Button]
        public void OpenLogFile()
        {
            logFilePath = Path.Combine(Application.persistentDataPath, "LogCapture.txt");
            System.Diagnostics.Process.Start(logFilePath);
        }

        public string GetAllLog => string.Join("\n", logMessages);
    }
}