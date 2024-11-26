using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace OSK
{
    [DisallowMultipleComponent]
    public class RotationProvider : DoTweenBaseProvider
    {
        public bool isLocal = true;
        public RotateMode rotateMode = RotateMode.Fast;

        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;

        private void Reset() => to = isLocal ? RootTransform.localEulerAngles : RootTransform.eulerAngles;

        public override Tweener InitTween()
        {
            RootTransform.localEulerAngles = from;
            return isLocal
                ? RootTransform.DOLocalRotate(to, settings.duration, rotateMode)
                : RootTransform.DORotate(to, settings.duration, rotateMode);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType,settings. useUnscaledTime)
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
            RootTransform.localEulerAngles = to;
            tweener = null;
        }
    }
}