using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    [CreateAssetMenu(fileName = "UIParticleSO", menuName = "OSK/UI/UIParticleSO", order = 0)]
    public class UIParticleSO : ScriptableObject
    {
        public EffectSetting[] EffectSettings => _effectSettings;
        [SerializeField] private EffectSetting[] _effectSettings;
    }
}
