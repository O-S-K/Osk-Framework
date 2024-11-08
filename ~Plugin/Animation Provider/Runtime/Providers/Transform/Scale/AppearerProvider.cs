using System;
using CustomInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace OSK
{
    public class AppearerProvider : DoTweenBaseProvider
    {
        [Header("Appearer")] 
        public float scaleInit = 0f;

        [Header("Hide")] 
        public Ease animEndButton = Ease.InBack;
        public float hideDelay;

        private bool shown;
        private Tween tween;

        public override Tweener InitTween()
        {
           return RootTransform.DOScale(Vector3.one, duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            RootTransform.localScale = Vector3.one * scaleInit;

            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(delay)
                .SetAutoKill(setAutoKill)
                .SetLoops(loopcount, loopType)
                .SetUpdate(isIgnoreTimeScale)
                .SetTarget(target)
                .OnComplete(
                    () =>
                    {
                        if (!shown)
                        {
                            shown = true;
                            onComplete?.Invoke();
                        }
                    });

            if (typeAnim == TypeAnimation.Ease)
                tweener.SetEase(ease);
            else
                tweener.SetEase(curve);
        }

        public void Hide()
        {
            tweener = RootTransform.DOScale(Vector3.zero, duration)
                .SetEase(animEndButton)
                .OnComplete(() =>
                {
                    if (shown)
                    {
                        shown = false;
                    }

                    gameObject.SetActive(false);
                });
        }

        public override void Stop()
        {
            base.Stop();
            tween?.Kill();
            shown = false;
        }

        [Button]
        public void HideWithDelay()
        {
            if (hideDelay <= 0)
                Hide();
            else
                tween = DOVirtual.DelayedCall(hideDelay, Hide);
        }
    }
}