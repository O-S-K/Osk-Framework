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

        public override void  ProgressTween()
        {
            _originalPosition = RootTransform.localPosition;
            _originalRotation = RootTransform.localEulerAngles;
            _originalScale = RootTransform.localScale;

            
            var rs = (isRandom) ? RandomUtils.RandomVector3(-strength, strength) : strength;
            tweener = typeShake switch
            {
                
                TypeShake.Position => RootTransform.DOShakePosition(settings.duration, rs, vibrato, randomness, snapping,
                    fadeOut),
                TypeShake.Rotation => RootTransform.DOShakeRotation(settings.duration, rs, vibrato, randomness, fadeOut),
                TypeShake.Scale => RootTransform.DOShakeScale(settings.duration, rs, vibrato, randomness, fadeOut),
                _ => null
            };
            base.ProgressTween();
        }


        public override void Play()
        {
            base.Play();
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