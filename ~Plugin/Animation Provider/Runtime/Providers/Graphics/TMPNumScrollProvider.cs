using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    [DisallowMultipleComponent]
    public class TMPNumScrollProvider : DoTweenBaseProvider
    {
        public Text text;
        public int startValue;
        public int endValue;
        
        public override Tweener InitTween()
        { 
            target =  text;
            return DOTween.To(() => 0, y => text.text = y.ToString(), endValue, duration); //tostring high GC
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;     
            
            text.text = startValue.ToString();
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

        private void Reset()
        {
            tweener?.Kill();
            tweener = null;
            text.text = startValue.ToString();
        }
    }
}