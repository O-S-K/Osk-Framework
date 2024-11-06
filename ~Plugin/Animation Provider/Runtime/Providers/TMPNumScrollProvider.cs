using DG.Tweening;
using TMPro;
using UnityEngine;

namespace OSK
{
    [DisallowMultipleComponent, RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPNumScrollProvider : DoTweenBaseProvider
    {
        public TextMeshProUGUI text;
        public int value;
        public override Tweener InitTween()
        {
            target = text;// This step must not be missed
            return DOTween.To(() => 0, y => text.text = y.ToString(), value, duration); //tostring high GC
        }
        private void Reset() => text = GetComponent<TextMeshProUGUI>();
    }
}