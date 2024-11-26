using System;
using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class RectPositionProvider : DoTweenBaseProvider
    {
        public bool snapping = false;
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;

        private void Reset()
        { 
            to = RootRectTransform.anchoredPosition;
        }

        public override Tweener InitTween()
        { 
            return RootRectTransform.DOAnchorPos(to, settings.duration, snapping);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            RootRectTransform.anchoredPosition = from;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType,settings.useUnscaledTime)
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
            tweener?.Rewind(); 
            RootRectTransform.anchoredPosition =  to;
            tweener = null;
        }
    }
}