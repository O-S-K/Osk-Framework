using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectDeltaSizeProvider : DoTweenBaseProvider
    {
        public bool snapping = false;
        public Vector3 to = Vector3.zero;
        public Vector3 from = Vector3.zero;
    
        private void Reset() => from = RootRectTransform.sizeDelta;
        
        public override Tweener InitTween()
        {
            target = RootRectTransform;
            return RootRectTransform.DOSizeDelta(from,settings. duration, snapping);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;   
            
            RootRectTransform.sizeDelta = from;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
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
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
        }
    }
}