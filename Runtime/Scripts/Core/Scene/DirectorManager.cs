using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.SceneManagement;

namespace OSK
{
    public class DirectorManager : GameFrameworkComponent
    {
        public enum State
        {
            Loading,
            Complete,
            Failed
        }

        [ReadOnly, SerializeField] private string _currentSceneName = "";

        public override void OnInit() { }

        public void LoadSceneFake(string sceneName, bool loadSceneAsync, float timeCompleted,
            LoadSceneMode loadSceneMode, Action<State, float> onLoadComplete)
        {
            LoadSceneFakeTime(sceneName, loadSceneAsync, timeCompleted, loadSceneMode, onLoadComplete).Run();
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode, Action<State> onLoadComplete)
        {
            if (HasScene(sceneName))
            {
                Logg.Log("Scene already loaded: " + sceneName);
            }
            
            Logg.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName, loadSceneMode);
            onLoadComplete?.Invoke(State.Complete);
        }

        public void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, Action<State> onLoadComplete)
        {
            if (HasScene(sceneName))
            {
                Logg.Log("Scene already loaded: " + sceneName);
            }
            LoadSceneAsyncCoroutine(sceneName, loadSceneMode, onLoadComplete).Run();
        }

        public void UnloadScene(string sceneName, Action<State> onUnloadComplete)
        {
            UnloadSceneAsyncCoroutine(sceneName, onUnloadComplete).Run();
        }

        public void LoadAllScenes(LoadSceneMode loadSceneMode)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                {
                    SceneManager.LoadScene(scene.name, loadSceneMode);
                }
            }
        } 

        public bool HasScene(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                return SceneManager.GetSceneAt(i).name == sceneName;
            }
            return false;
        }
        
        public void RestartScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        private IEnumerator LoadSceneFakeTime(string sceneName, bool loadSceneAsync, float timeCompleted,
            LoadSceneMode loadSceneMode, Action<State, float> onLoadComplete)
        {
            float percent = timeCompleted / 100;
            // percent Max = 1 (100%)
            while (percent < 1)
            {
                percent += Time.deltaTime / timeCompleted;
                onLoadComplete?.Invoke(State.Loading, percent);
                yield return null;
            }

            string scene = sceneName;
            if (int.TryParse(sceneName, out int sceneIndex))
            {
                scene = SceneManager.GetSceneByBuildIndex(sceneIndex).name;
            }

            if (loadSceneAsync)
            {
                var _scene = SceneManager.LoadSceneAsync(scene, loadSceneMode);
                if (_scene.isDone)
                {
                    // percent Max = 1 (100%)
                    onLoadComplete?.Invoke(State.Complete, 1);
                }
            }
            else
            {
                // percent Max = 1 (100%)
                SceneManager.LoadScene(scene, loadSceneMode);
                onLoadComplete?.Invoke(State.Complete, 1);
            }
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, LoadSceneMode loadSceneMode,
            Action<State> onLoadComplete)
        {
            onLoadComplete?.Invoke(State.Loading);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            if (asyncLoad == null)
            {
                Logg.Log("Scene failed to load: " + sceneName);
                onLoadComplete?.Invoke(State.Failed);
                yield break;
            }
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                Logg.Log("Loading progress: " + asyncLoad.progress);
                yield return null;

                if (asyncLoad.progress >= 0.9f)
                    asyncLoad.allowSceneActivation = true;
            }

            if (asyncLoad.isDone)
            {
                Logg.Log("Scene loaded and activated: " + sceneName);
                onLoadComplete?.Invoke(State.Complete);
            }
            else
            {
                Logg.Log("Scene failed to load: " + sceneName);
                onLoadComplete?.Invoke(State.Failed);
            }
        }

        private IEnumerator UnloadSceneAsyncCoroutine(string sceneName, Action<State> onUnloadComplete)
        {
            string scene = sceneName;
            if (int.TryParse(sceneName, out int sceneIndex))
            {
                scene = SceneManager.GetSceneByBuildIndex(sceneIndex).name;
            }
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);
            while (asyncUnload != null && !asyncUnload.isDone)
            {
                Logg.Log("Unloading progress: " + asyncUnload.progress);
                yield return null;
            }

            Logg.Log("Scene unloaded: " + sceneName);
            onUnloadComplete?.Invoke(State.Complete);
        }
    }
}