using UnityEngine;

namespace OSK
{
    public abstract class SingletonScene<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var found = Object.FindObjectsOfType<T>();

                if (found.Length > 1)
                {
                    Debug.LogWarning($"[SingletonScene<{typeof(T).Name}>] Multiple instances found. Destroying extras.");
                    for (int i = 1; i < found.Length; i++)
                        Object.Destroy(found[i].gameObject);
                }

                _instance = found.Length > 0 ? found[0] : new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }
    }
}