using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class DotweenProviderManager : MonoBehaviour
    {
        public List<IDoTweenProviderBehaviours> Providers => new List<IDoTweenProviderBehaviours>(
            GetComponentsInChildren<IDoTweenProviderBehaviours>());
    }
}