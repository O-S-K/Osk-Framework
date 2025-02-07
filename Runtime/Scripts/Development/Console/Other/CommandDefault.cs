using UnityEngine;

namespace OSK
{
    public class CommandDefault : MonoBehaviour
    {
        [ConsoleCommand("cs_set_fps")]
        public void SetFPS(int fps)
        {
            Application.targetFrameRate = fps;
            Debug.Log("FPS set to: " + fps);
        }

        [ConsoleCommand("cs_fps")]
        public void FPS(bool isShow, bool showMemory = true)
        {
            if(isShow)
            {
                if (FindObjectOfType<FPSCounter>() != null)
                {
                    Debug.Log("FPS counter already exists");
                    return;
                }

                var fpsCounter = new GameObject("FPS Counter");
                DontDestroyOnLoad(fpsCounter);
                fpsCounter.AddComponent<FPSCounter>();
                fpsCounter.GetComponent<FPSCounter>().isShowMemoryStatus = showMemory;
            }
            else
            {
                if (FindObjectOfType<FPSCounter>() != null)
                {
                    Destroy(FindObjectOfType<FPSCounter>().gameObject);
                }
                else
                {
                    Debug.Log("FPS counter not found");
                }
            }
        }

        [ConsoleCommand("cs_info_system")]
        public void InfoSystem(bool showSystemInfo = true)
        {
            if(showSystemInfo)
            {
                if (FindObjectOfType<DeviceInfo>() != null)
                {
                    Debug.Log("Device info already exists");
                    return;
                }

                var deviceInfo = new GameObject("Device Info");
                DontDestroyOnLoad(deviceInfo);
                deviceInfo.AddComponent<DeviceInfo>();
            }
            else
            {
                if (FindObjectOfType<DeviceInfo>() != null)
                {
                    Destroy(FindObjectOfType<DeviceInfo>().gameObject);
                }
                else
                {
                    Debug.Log("Device info not found");
                }
            }
        }

        [ConsoleCommand("cs_set_time_scale")]
        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
            Debug.Log("Time scale set to: " + timeScale);
        }

        [ConsoleCommand("cs_quit")]
        public void SetQuit()
        {
            Application.Quit();
            Debug.Log("Application quit");
        }

        [ConsoleCommand("cs_reset_scene")]
        public void SceneReset()
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            Debug.Log("Scene reset");
        }

        [ConsoleCommand("cs_load_scene")]
        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            Debug.Log("Scene loaded: " + sceneName);
        }

        [ConsoleCommand("cs_remove_key_command")]
        public void RemoveKeyCommand(string command)
        {
            ConsoleGUI.RemoveCommand(command);
            Debug.Log("Key command removed: " + command);
        }
        
        [ConsoleCommand("cs_show_debug_log")]
        public void ShowDebugLog()
        {
            ConsoleTool.Instance.popupDebugLog.OpenWindow();
        }
         
        [ConsoleCommand("cs_show_logg")]
        public void ShowLogg()
        {
            ConsoleTool.Instance.popupDebugLog.OpenWindow();
            ConsoleTool.Instance.popupDebugLog.AddLog(LogCapture.Instance.GetAllLog);
        }
    }
}