using UnityEngine;
using CustomInspector;
using System.Collections;
using UnityEngine.SceneManagement;

namespace OSK
{
    public class SceneManager : GameFrameworkComponent
    {
        public System.Action OnLoadingStart { get; set; }
        public System.Action<float> OnLoadingProgress { get; set; }
        public System.Action OnLoadingComplete { get; set; }
        public System.Action<string> OnLoadingFailed { get; set; }
        
        [ReadOnly, SerializeField] private string currentSceneName = "";


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

private IEnumerator LoadSceneAsyncFake(string sceneName, LoadSceneMode loadSceneMode)
{
    // Trigger start loading event
    OnLoadingStart?.Invoke();

    AsyncOperation asyncLoad =
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
    
    // Prevent the scene from activating immediately
    asyncLoad.allowSceneActivation = false;

    // Fake progress
    float fakeProgress = 0f;

    // While the operation is not done
    while (!asyncLoad.isDone)
    {
        // Increase fake progress over time (example: 1% per frame)
        if (fakeProgress < 0.9f) // You can adjust the limit as needed
        {
            fakeProgress += Time.deltaTime * 0.1f; // Speed of progress increment
        }
        else if (asyncLoad.progress >= 0.9f)
        {
            // When real asyncLoad.progress is almost done, finish fake progress
            fakeProgress = 1f;
        }

        // Update progress feedback with the fake progress value
        OnLoadingProgress?.Invoke(fakeProgress);
        OSK.Logg.Log("Fake loading progress: " + fakeProgress);

        yield return null;

        // Once the scene is ready and fake progress reaches 1, activate it
        if (fakeProgress >= 1f && asyncLoad.progress >= 0.9f)
        {
            asyncLoad.allowSceneActivation = true;
        }
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