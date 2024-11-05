using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    [CreateAssetMenu(fileName = "UIImageSO", menuName = "OSK/UI/UIImageSO", order = 0)]
    public class UIImageSO : ScriptableObject
    {
        public EffectSetting[] EffectSettings => _effectSettings;
        [SerializeField] private EffectSetting[] _effectSettings;
    }
}
