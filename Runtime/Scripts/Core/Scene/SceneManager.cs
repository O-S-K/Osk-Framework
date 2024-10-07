using UnityEngine;
using CustomInspector;
using System.Collections;
using UnityEngine.SceneManagement;

namespace OSK
{
    public class SceneManager : GameFrameworkComponent
    {
        public System.Action OnLoadingStart;
        public System.Action<float> OnLoadingProgress;
        public System.Action OnLoadingComplete;
        public System.Action<string> OnLoadingFailed;
        [ReadOnly, SerializeField] private string currentSceneName;


        public void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (HasScene(sceneName))
            {
                OSK.Logg.Log("Scene already loaded: " + sceneName);
                OnLoadingFailed?.Invoke("Scene already loaded: " + sceneName);
            }
            else
            {
                StartCoroutine(LoadSceneAsyncCoroutine(sceneName, loadSceneMode));
            }
        }

        public void LoadSceneOnTime(string sceneName, float timeCompleted)
        {
            StartCoroutine(IELoadSceneOnTime(sceneName, timeCompleted));

            IEnumerator IELoadSceneOnTime(string sceneName, float timeCompleted)
            {
                OnLoadingStart?.Invoke();
                yield return new WaitForSeconds(timeCompleted);
                OnLoadingComplete?.Invoke();
            }
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            OnLoadingStart?.Invoke();
            if (HasScene(sceneName))
            {
                OSK.Logg.Log("Scene already loaded: " + sceneName);
                OnLoadingFailed?.Invoke("Scene already loaded: " + sceneName);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, loadSceneMode);
                OnLoadingComplete?.Invoke();
            }
        }
        
        public void LoadAllScenes()
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name);
                }
            }
        }

        private bool HasScene(string sceneName)
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

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, LoadSceneMode loadSceneMode)
        {
            // Trigger start loading event
            OnLoadingStart?.Invoke();

            AsyncOperation asyncLoad =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            // Check if the operation is done
            while (!asyncLoad.isDone)
            {
                // You can add progress feedback here if needed
                OnLoadingProgress?.Invoke(asyncLoad.progress);
                OSK.Logg.Log("Loading progress: " + asyncLoad.progress);
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

        public void UnloadScene(string sceneName)
        {
            StartCoroutine(UnloadSceneAsyncCoroutine(sceneName));
        }

        private IEnumerator UnloadSceneAsyncCoroutine(string sceneName)
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

            // Wait for the scene to be unloaded
            while (!asyncUnload.isDone)
            {
                // You can add progress feedback here if needed
                OSK.Logg.Log("Unloading progress: " + asyncUnload.progress);
                yield return null;
            }

            // Scene successfully unloaded
            OSK.Logg.Log("Scene unloaded: " + sceneName);
        }
    }
}