using UnityEngine;
using System;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public abstract class DIContextInject : MonoBehaviour
    {
        public GameObject[] autoInjectGameObjects;
        public static event Action OnDependenciesInjected;

        protected virtual void Awake()
        { 
            AutoBindings();
            DIContainer.InstallBindAndInjects();
            OnDependenciesInjected?.Invoke();
        }

        private void AutoBindings()
        {
            foreach (var gameObjectInject in autoInjectGameObjects)
            {
                if (gameObjectInject == null)
                    continue;
                DIContainer.BindFromPrefab(gameObjectInject);
            }
            InstallBindings();
        }

        public abstract void InstallBindings();
    }
}