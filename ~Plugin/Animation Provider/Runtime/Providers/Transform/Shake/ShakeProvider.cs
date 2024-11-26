using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public enum TypeShake
    {
        Position,
        Rotation,
        Scale
    }

    public class ShakeProvider : DoTweenBaseProvider
    {
        public TypeShake typeShake = TypeShake.Position;
        public bool isRandom = false;
        public Vector3 strength = new Vector3(1, 1, 1);
        public int vibrato = 10;
        public float randomness = 90;
        public bool snapping = false;
        public bool fadeOut = true;


        private Vector3 _originalPosition;
        private Vector3 _originalRotation;
        private Vector3 _originalScale;

        public override Tweener InitTween()
        {
            var rs = (isRandom) ? RandomUtils.RandomVector3(-strength, strength) : strength;
            return typeShake switch
            {
                
                TypeShake.Position => RootTransform.DOShakePosition(settings.duration, rs, vibrato, randomness, snapping,
                    fadeOut),
                TypeShake.Rotation => RootTransform.DOShakeRotation(settings.duration, rs, vibrato, randomness, fadeOut),
                TypeShake.Scale => RootTransform.DOShakeScale(settings.duration, rs, vibrato, randomness, fadeOut),
                _ => null
            };
        }

        public override void Play()
        {
            _originalPosition = RootTransform.localPosition;
            _originalRotation = RootTransform.localEulerAngles;
            _originalScale = RootTransform.localScale;

            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;

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
            tweener?.Rewind();
            tweener = null;

            RootTransform.localPosition = _originalPosition;
            RootTransform.localEulerAngles = _originalRotation;
            RootTransform.localScale = _originalScale;
        }
    }
}