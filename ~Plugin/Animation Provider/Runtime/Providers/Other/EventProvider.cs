using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace OSK
{
    public class EventProvider : DoTweenBaseProvider
    {
        public UnityEvent onComplete;
        public bool from;
        public bool to;

        private Tween tween;

        public override Tweener InitTween()
        {
            return DOVirtual.Float(from ? 1 : 0, to ? 1 : 0, settings.duration, value =>
            {
                if (value > 0)
                    onComplete?.Invoke();
            });
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            onComplete.RemoveAllListeners();

            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetUpdate(settings.useUnscaledTime)
                .SetTarget(target)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void Stop()
        {
            base.Stop();
            onComplete.RemoveAllListeners();
        }
    }
}