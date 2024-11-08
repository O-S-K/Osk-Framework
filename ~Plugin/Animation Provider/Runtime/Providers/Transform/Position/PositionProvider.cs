using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class PositionProvider : DoTweenBaseProvider
    {
        public bool isLocal = true;
        public bool snapping = false;
        public bool negatives = false;
        public Vector3 endValue = Vector3.zero;
        private void Reset() => endValue = isLocal ? transform.localPosition : transform.position;


        public override Tweener InitTween()
        {
            return isLocal ? transform.DOLocalMove(endValue, duration, snapping) : transform.DOMove(endValue, duration, snapping);
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
                .SetRelative(negatives)
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