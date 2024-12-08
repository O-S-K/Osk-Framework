using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace OSK
{
    public static class CoroutineExtension
    {
        public static CoroutineHandler Run(this IEnumerator enumerator)
        {
            var handler = new CoroutineHandler(enumerator);
            handler.Start();
            return handler;
        }
    }

    public class CoroutineHandler
    {
        public IEnumerator Coroutine { get; private set; }
        public bool Paused { get; private set; }
        public bool Running { get; private set; }
        public bool Stopped { get; private set; }

        // Event for Completion
        public UnityEvent<bool> OnCompleted { get; } = new UnityEvent<bool>();

        public CoroutineHandler(IEnumerator coroutine)
        {
            Coroutine = coroutine ?? throw new System.ArgumentNullException(nameof(coroutine));
        }

        // Control Methods
        public void Start()
        {
            if (!Running && Coroutine != null)
            {
                Running = true;
                CoroutineDriver.Run(CallWrapper());
            }
        }

        public void Stop()
        {
            Stopped = true;
            Running = false;
        }

        public void Pause() => Paused = true;

        public void Resume() => Paused = false;

        public CoroutineHandler OnComplete(UnityAction<bool> action)
        {
            OnCompleted.AddListener(action);
            return this;
        }

        // Cleanup Method
        private void Finish()
        {
            OnCompleted.Invoke(Stopped);
            OnCompleted.RemoveAllListeners();
            Coroutine = null;
        }

        // Internal Coroutine Wrapper
        private IEnumerator CallWrapper()
        {
            yield return null;

            while (Running)
            {
                if (Paused)
                {
                    yield return null;
                }
                else if (Coroutine?.MoveNext() == true)
                {
                    yield return Coroutine.Current;
                }
                else
                {
                    Running = false;
                }
            }

            Finish();
        }

        internal class CoroutineDriver : MonoBehaviour
        {
            private static CoroutineDriver instance;

            public static Coroutine Run(IEnumerator target) => Instance.StartCoroutine(target);

            private static CoroutineDriver Instance
            {
                get
                {
                    if (instance == null)
                    {
                        var go = new GameObject("[CoroutineDriver]");
                        instance = go.AddComponent<CoroutineDriver>();
                        DontDestroyOnLoad(go);
                    }
                    return instance;
                }
            }

            private void Awake()
            {
                if (instance != null && instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
