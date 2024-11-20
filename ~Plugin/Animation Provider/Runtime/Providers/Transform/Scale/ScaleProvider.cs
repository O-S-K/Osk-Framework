using System;
using CustomInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OSK
{
    public class ScaleProvider : DoTweenBaseProvider
    {
        [Header("Appearer")] 
        public Vector3 startValue = Vector3.zero;
        public Vector3 endValue = Vector3.one;

        [Header("Hide")] 
        public Ease animEndButton = Ease.InBack;
        public float hideDelay;

        private Tween tween;

        public override Tweener InitTween()
        {
           return RootTransform.DOScale(endValue, duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            RootTransform.localScale = startValue;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(delay)
                .SetAutoKill(setAutoKill)
                .SetLoops(loopcount, loopType)
                .SetUpdate(isIgnoreTimeScale)
                .SetTarget(target)
                .OnComplete(DoneTween);

            if (typeAnim == TypeAnimation.Ease)
                tweener.SetEase(ease);
            else
                tweener.SetEase(curve);
        }

        private void DoneTween()
        {
            Debug.Log($"onComplete has listeners: {onComplete.GetPersistentEventCount()}");
                onComplete?.Invoke();
        }

        public void Hide()
        {
            tweener = RootTransform.DOScale(Vector3.zero, duration)
                .SetEase(animEndButton)
                .OnComplete(() =>
                { 
                    gameObject.SetActive(false);
                });
        }

        public override void Stop()
        {
            base.Stop();
            tween?.Kill(); 
            RootTransform.localScale = endValue;
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