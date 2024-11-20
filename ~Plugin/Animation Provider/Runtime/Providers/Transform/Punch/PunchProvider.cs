using CustomInspector;
using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public enum TypePunch
    {
        Position,
        Rotation,
        RotationBlend,
        Scale
    }

    public class PunchProvider : DoTweenBaseProvider
    {
        public TypePunch typeShake = TypePunch.Position;
        public bool isRandom = false;
        public Vector3 strength = new Vector3(1, 1, 1);
        public int vibrato = 10;
        public float elasticity = 1;
        public bool snapping = false;


        private Vector3 _originalPosition;
        private Vector3 _originalRotation;
        private Vector3 _originalScale;

        public override Tweener InitTween()
        {
            var rs = (isRandom) ? RandomUtils.RandomVector3(-strength, strength) : strength;
            return typeShake switch
            {
                TypePunch.Position => RootTransform.DOPunchPosition(rs, duration, vibrato, elasticity, snapping),
                TypePunch.Rotation => RootTransform.DOPunchRotation(rs, duration, vibrato, elasticity),
                TypePunch.RotationBlend => RootTransform.DOBlendablePunchRotation(rs, duration, vibrato, elasticity),
                TypePunch.Scale =>  RootTransform.DOPunchScale(rs, duration, vibrato, elasticity),
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
            tweener?.Rewind();
            tweener = null;

            RootTransform.localPosition = _originalPosition;
            RootTransform.localEulerAngles = _originalRotation;
            RootTransform.localScale = _originalScale;
        }
    }
}