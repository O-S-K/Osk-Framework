using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    [DisallowMultipleComponent]
    public class FillAmountProvider : DoTweenBaseProvider
    {
        public Image image;
        [Range(0, 1)]
        public float startValue;
        
        [Range(0, 1)]
        public float endValue;
        
        public override Tweener InitTween()
        { 
            target =  image;
            return DOTween.To(() => 0, y => image.fillAmount = (float)y, endValue, duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;     
            
            image.fillAmount = startValue;
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
            image.fillAmount = startValue;
        }
    }
}