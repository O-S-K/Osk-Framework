using UnityEngine;

namespace OSK
{
    public class DeviceInfo : MonoBehaviour
    {
        private bool _showSystemInfo = true;
        private Rect _windowRect;

        private const float windowWidth = 400f;
        private const float windowHeight = 150f;

        private void Start()
        {
            _windowRect = new Rect(10, 10, windowWidth, windowHeight); 
        }

        private void OnGUI()
        {
            if (_showSystemInfo)
            {
                _windowRect = GUI.Window(0, _windowRect, DrawSystemInfoWindow, "System Information");
            }
        }

        private void DrawSystemInfoWindow(int windowID)
        {
            string systemInfo = $"OS: {SystemInfo.operatingSystem}\n" +
                                $"Processor: {SystemInfo.processorType}\n" +
                                $"Processor Count: {SystemInfo.processorCount}\n" +
                                $"Memory: {SystemInfo.systemMemorySize} MB\n" +
                                $"Graphics: {SystemInfo.graphicsDeviceName}\n" +
                                $"Graphics Memory: {SystemInfo.graphicsMemorySize} MB";

            GUI.Label(new Rect(10, 30, windowWidth - 20, windowHeight - 40), systemInfo);
            if (GUI.Button(new Rect(windowWidth - 50, 0, 50, 20), "Close"))
            {
                _showSystemInfo = false;
            }
            GUI.DragWindow(new Rect(0, 0, windowWidth, 20));
        }
    }
}