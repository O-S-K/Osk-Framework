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

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;            
            tweener = InitTween();
            tweener.SetDelay(delay)
                .SetAutoKill(setAutoKill)
                .SetLoops(loopcount, loopType)
                .SetUpdate(isIgnoreTimeScale)
                .SetTarget(target)
                .OnComplete(() => onComplete?.Invoke());

            if (typeAnim == TypeAnimation.Ease)
                tweener.SetEase(ease);
            else
                tweener.SetEase(curve);
        }

        private void Reset() => text = GetComponent<TextMeshProUGUI>();
    }
}