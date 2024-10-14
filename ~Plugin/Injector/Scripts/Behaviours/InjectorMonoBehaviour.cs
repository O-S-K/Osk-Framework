using UnityEngine;

namespace Injector
{
    public class InjectorMonoBehaviour : MonoBehaviour
    {
        private bool isInjected = false;
        public bool IsInjected
        {
            get
            {
                return isInjected;
            }
        }

        protected virtual void Awake()
        {
            Resolver.Resolve(gameObject, this);
            isInjected = true;
        }
    }
}