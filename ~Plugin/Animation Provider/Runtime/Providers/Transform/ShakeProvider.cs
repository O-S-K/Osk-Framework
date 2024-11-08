using CustomInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

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
            return typeShake switch
            {
                TypeShake.Position => RootTransform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut),
                TypeShake.Rotation => RootTransform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut),
                TypeShake.Scale => RootTransform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut),
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