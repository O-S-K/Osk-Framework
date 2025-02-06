using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OSK
{
    public class ConsoleTool : MonoBehaviour
    {
        public RectTransform canvasTR;
        public GameObject popupConsole;
        public DebugLogWindow popupDebugLog;
        public bool showConsole = true;
        public float popupOpacity = 1f;
        public bool popupAvoidsScreenCutout = true;

        public Font Font;
        public Color BackgroundColor = new Color(0, 0, 0, .65f);
        private bool isConsoleVisible = false;
        private Canvas canvas;

        private void OnEnable()
        {
            InputPreprocessor.AddPreprocessor(PreprocessInput);
        }

        private void OnDisable()
        {
            InputPreprocessor.RemovePreprocessor(PreprocessInput);
        }

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            float newRatio = (float)Screen.width / Screen.height;
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = newRatio > 0.65f ? 1 : 0.3f;

            ConsoleGUI.Font = Font;
            ConsoleGUI.BackgroundColor = BackgroundColor;
            ConsoleGUI.Initialize();

            if (popupConsole != null)
            {
                popupConsole.SetActive(showConsole);
            }
            popupDebugLog.CloseWindow();
        }

        private string PreprocessInput(string input)
        {
            if (input != null && input.StartsWith("@"))
            {
                Debug.Log(input);
                return null;
            }

            return input;
        }

        public void ShowConsoleWindow()
        {
            isConsoleVisible = !isConsoleVisible;
            if (isConsoleVisible)
            {
                ConsoleGUI.Show();
            }
            else
            {
                ConsoleGUI.Hide();
            }
        }
        
        public void ShowPopupDebugLog()
        {
            popupDebugLog.OpenWindow();
        }
        
        public void HidePopupDebugLog()
        {
            popupDebugLog.CloseWindow();
        }
    }
}