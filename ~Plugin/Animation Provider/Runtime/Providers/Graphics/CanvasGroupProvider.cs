using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class CanvasGroupProvider : DoTweenBaseProvider
    {
        private CanvasGroup canvasGroup;
        public float from = 0;
        public float to = 1;


        #region Editor initialization and null reference prevention measures

        private void Reset()
        {
            canvasGroup = gameObject.GetOrAdd<CanvasGroup>();
            to = canvasGroup.alpha;
        }

        #endregion

        public override Tweener InitTween()
        {
            return canvasGroup.DOFade(to, settings.duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;

            canvasGroup = gameObject.GetOrAdd<CanvasGroup>();
            canvasGroup.alpha = from;

            tweener = InitTween();

            tweener
                .SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType, settings.useUnscaledTime)
                .SetTarget(target)
                .OnComplete(() => settings.eventCompleted?.Invoke());

            if (settings.typeAnim == TypeAnimation.Ease)
                tweener.SetEase(settings.ease);
            else
                tweener.SetEase(settings.curve);
        }

        public override void Stop()
        {
            base.Stop();
            canvasGroup.alpha = to;
        }
    }
}