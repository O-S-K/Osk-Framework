using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OSK
{
    public class SceneManager : GameFrameworkComponent
    {
        public static SceneManager Instance { get; private set; }
 
        public static System.Action OnLoadingStart;
        public static System.Action OnLoadingLoop;
        public static System.Action OnLoadingComplete;
        public static System.Action<string> OnLoadingFailed;
        
        private string currentSceneName;

        private void Awake()
        {
            Instance = this;
        }

        public static void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if(HasScene(sceneName))
            {
                Debug.Log("Scene already loaded: " + sceneName);
                OnLoadingFailed?.Invoke("Scene already loaded: " + sceneName);
            }
            else
            {
                Instance.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, loadSceneMode));
            }
        }
        
        public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            OnLoadingStart?.Invoke();
            if(HasScene(sceneName))
            {
                Debug.Log("Scene already loaded: " + sceneName);
                OnLoadingFailed?.Invoke("Scene already loaded: " + sceneName);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, loadSceneMode);
                OnLoadingComplete?.Invoke();
            }
        }
        
        private static bool HasScene(string sceneName)
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerator LoadSceneAsyncCoroutine(string sceneName, LoadSceneMode loadSceneMode)
        {
            // Trigger start loading event
            OnLoadingStart?.Invoke();

            AsyncOperation asyncLoad =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            // Check if the operation is done
            while (!asyncLoad.isDone)
            {
                // You can add progress feedback here if needed
                OnLoadingLoop?.Invoke();
                Debug.Log("Loading progress: " + asyncLoad.progress);
                yield return null;
            }

            // Check if scene loaded successfully
            if (asyncLoad.isDone)
            {
                // Trigger complete loading event
                OnLoadingComplete?.Invoke();
            }
            else
            {
                // Trigger failed loading event
                OnLoadingFailed?.Invoke("Failed to load scene: " + sceneName);
            }
        }
 
        public static void UnloadScene(string sceneName)
        {
            Instance.StartCoroutine(UnloadSceneAsyncCoroutine(sceneName));
        }
        private static IEnumerator UnloadSceneAsyncCoroutine(string sceneName)
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

            // Wait for the scene to be unloaded
            while (!asyncUnload.isDone)
            {
                // You can add progress feedback here if needed
                Debug.Log("Unloading progress: " + asyncUnload.progress);
                yield return null;
            }

            // Scene successfully unloaded
            Debug.Log("Scene unloaded: " + sceneName);
        }
    }
}