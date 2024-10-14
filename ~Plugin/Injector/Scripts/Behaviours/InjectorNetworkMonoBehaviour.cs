#if UNITY_2018_OR_EARLIER
using UnityEngine.Networking;

namespace Injector
{
    public class InjectorNetworkMonoBehaviour : NetworkBehaviour
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
#endif