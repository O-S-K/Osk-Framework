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
        [SerializeField] private Button _refreshButton;

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
            _refreshButton.onClick.AddListener(Refresh);
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
            _content.gameObject.SetActive(true);
            
        }

        public void CloseWindow()
        {
            _content.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        
        public void Submit()
        {
        }
        
        public void Refresh()
        {
            if (string.IsNullOrEmpty(logMessage))
            {
                _textMessage.text = "Log is empty";
                return;
            } 
            _textMessage.text = logMessage;
        }

        public void ClearLog()
        {
            _textMessage.text = string.Empty;
            logMessage = string.Empty;
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