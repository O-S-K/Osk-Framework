using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.SceneManagement;

namespace OSK
{
    public class SceneManager : GameFrameworkComponent
    {
        public enum State
        {
            Loading,
            Complete,
            Failed
        }

        [ReadOnly, SerializeField] private string _currentSceneName = "";

        public override void OnInit() { }

        public void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, Action<State> onLoadComplete)
        {
            if (HasScene(sceneName))
            {
                OSK.Logg.Log("Scene already loaded: " + sceneName);
                onLoadComplete?.Invoke(State.Failed);
            }
            else
            {
                LoadSceneAsyncCoroutine(sceneName, loadSceneMode, onLoadComplete).Run();
            }
        }

        public void LoadSceneOnTime(string sceneName, float timeCompleted, Action<State, float> onLoadComplete)
        {
            LoadSceneFakeTime(sceneName, timeCompleted, LoadSceneMode.Single, onLoadComplete).Run();
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode, Action<State> onLoadComplete)
        {
            if (HasScene(sceneName))
            {
                OSK.Logg.Log("Scene already loaded: " + sceneName);
                onLoadComplete?.Invoke(State.Failed);
            }
            else
            {
                LoadSceneAsyncCoroutine(sceneName, loadSceneMode, onLoadComplete).Run();
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
        
        public IEnumerator LoadSceneFakeTime(string sceneName, float timeCompleted, LoadSceneMode loadSceneMode, Action<State, float> onLoadComplete)
        {
              float percent = timeCompleted / 100;
              while (percent < 1)
              {
                  percent += Time.deltaTime / timeCompleted;
                  onLoadComplete?.Invoke(State.Loading, percent);
                  yield return null;
              } 
              
              onLoadComplete?.Invoke(State.Complete, 1);
              UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, LoadSceneMode loadSceneMode, Action<State> onLoadComplete)
        {
            onLoadComplete?.Invoke(State.Loading);
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                OSK.Logg.Log("Loading progress: " + asyncLoad.progress);
                yield return null;

                if (asyncLoad.progress >= 0.9f) 
                    asyncLoad.allowSceneActivation = true; 
            }
            if (asyncLoad.isDone)
            {
                OSK.Logg.Log("Scene loaded and activated: " + sceneName);
                onLoadComplete?.Invoke(State.Complete);
            }
            else
            {
                OSK.Logg.Log("Scene failed to load: " + sceneName);
                onLoadComplete?.Invoke(State.Failed);
            }
        }

        public void UnloadScene(string sceneName, Action<State> onUnloadComplete)
        {
            UnloadSceneAsyncCoroutine(sceneName, onUnloadComplete).Run();
        }

        private IEnumerator UnloadSceneAsyncCoroutine(string sceneName, Action<State> onUnloadComplete)
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

            while (!asyncUnload.isDone)
            {
                OSK.Logg.Log("Unloading progress: " + asyncUnload.progress);
                yield return null;
            }

            OSK.Logg.Log("Scene unloaded: " + sceneName);
            onUnloadComplete?.Invoke(State.Complete);
        }
    }
}