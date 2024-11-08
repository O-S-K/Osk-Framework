using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectDeltaSizeProvider : DoTweenBaseProvider
    {
        public bool snapping = false;
        public Vector3 endValue = Vector3.zero;
    
        private void Reset() => endValue = RootRectTransform.sizeDelta;
        
        public override Tweener InitTween()
        {
            target = RootRectTransform;
            return RootRectTransform.DOSizeDelta(endValue, duration, snapping);
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

        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
        }
    }
}