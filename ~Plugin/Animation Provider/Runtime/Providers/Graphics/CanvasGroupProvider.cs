using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class CanvasGroupProvider : DoTweenBaseProvider
    {
        private CanvasGroup canvasGroup;
        public float startValue = 0;
        public float endValue = 1;


        #region Editor initialization and null reference prevention measures

        private void Reset()
        {
            canvasGroup = gameObject.GetOrAdd<CanvasGroup>();
            endValue = canvasGroup.alpha;
        }

        #endregion

        public override Tweener InitTween()
        {
            return canvasGroup.DOFade(endValue, duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;

            canvasGroup = gameObject.GetOrAdd<CanvasGroup>();
            canvasGroup.alpha = startValue;

            tweener = InitTween();

            tweener
                .SetDelay(delay)
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
            canvasGroup.alpha = endValue;
        }
    }
}