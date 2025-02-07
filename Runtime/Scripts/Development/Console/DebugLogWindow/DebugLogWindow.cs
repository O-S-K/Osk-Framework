using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class DebugLogWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMessage;
        [SerializeField] private RectTransform _content;
        
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private ScrollRect _scrollRect;

        [Header("Buttons")]
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _clearButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _exportButton;
        [SerializeField] private Button _openFileButton;

        private string fileName = "DebugWindow.txt";
        private string logMessage;
        private string filePath;
        
        private void Start()
        {
            _submitButton.onClick.AddListener(Submit);
            _clearButton.onClick.AddListener(ClearLog);
            _closeButton.onClick.AddListener(CloseWindow);
            _exportButton.onClick.AddListener(ExportLog);
            _openFileButton.onClick.AddListener(OpenFile);
        }

        public void AddLog(string message)
        {
            logMessage = message;
            _textMessage.text = message;
        }

        public void OpenWindow()
        {
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            _scrollRect.verticalNormalizedPosition = 0;
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
        
        public void Submit()
        {
              
        }

        public void ClearLog()
        {
            _textMessage.text = string.Empty;
        }

        private void ExportLog()
        {
            if (string.IsNullOrEmpty(logMessage))
            {
                _textMessage.text = "Log is empty";
                return;
            }

            filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(filePath, logMessage);
            Logg.Log($"Log exported to: {filePath}");
        }

        private void OpenFile()
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath);
            }
            else
            {
                Logg.Log("Log file not found");
            }
        }
        
        private void DeleteFile()
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Logg.Log("Log file deleted");
            }
        }
    }
}