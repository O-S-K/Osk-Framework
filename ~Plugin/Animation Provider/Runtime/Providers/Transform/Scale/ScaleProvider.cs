using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OSK
{
    public class ScaleProvider : DoTweenBaseProvider
    {
        [Header("Appearer")] 
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.one;

        [Header("Hide")] 
        public Ease animEndButton = Ease.InBack;
        public float hideDelay;

        private Tween tween;

        public override Tweener InitTween()
        {
           return RootTransform.DOScale(to, settings.duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            RootTransform.localScale = from;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType, settings.useUnscaledTime)
                .SetTarget(target)
                .OnComplete( () => settings.eventCompleted?.Invoke());

            if (settings.typeAnim == TypeAnimation.Ease)
                tweener.SetEase(settings.ease);
            else
                tweener.SetEase(settings.curve);
        }

        

        public void Hide()
        {
            tweener = RootTransform.DOScale(Vector3.zero, settings.duration)
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
            RootTransform.localScale = to;
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