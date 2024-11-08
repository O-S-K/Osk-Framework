using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class RotationProvider : DoTweenBaseProvider
    {
        public bool isLocal = true;
        public RotateMode rotateMode = RotateMode.Fast;
        public Vector3 endValue = Vector3.zero;

        private void Reset() => endValue = isLocal ? RootTransform.localEulerAngles : RootTransform.eulerAngles;

        public override Tweener InitTween()
        {
            return isLocal
                ? RootTransform.DOLocalRotate(endValue, duration, rotateMode)
                : RootTransform.DORotate(endValue, duration, rotateMode);
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