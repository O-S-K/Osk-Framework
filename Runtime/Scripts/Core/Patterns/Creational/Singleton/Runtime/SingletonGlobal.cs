using UnityEngine;

namespace OSK
{
    public abstract class SingletonGlobal<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected virtual bool IsDontDestroySingleton => true;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var found = Object.FindObjectsOfType<T>();

                if (found.Length > 1)
                {
                    Logg.LogWarning(
                        $"[SingletonGlobal<{typeof(T).Name}>] Multiple instances found. Destroying extras.");
                    for (int i = 1; i < found.Length; i++)
                        Object.Destroy(found[i].gameObject);
                }

                _instance = found.Length > 0 ? found[0] : new GameObject(typeof(T).Name).AddComponent<T>();

                if ((_instance as SingletonGlobal<T>)?.IsDontDestroySingleton == true)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
    }
}