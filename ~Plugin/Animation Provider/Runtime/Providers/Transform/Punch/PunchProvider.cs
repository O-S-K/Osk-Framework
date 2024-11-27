using Sirenix.OdinInspector;
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

        public override void ProgressTween()
        {
            _originalPosition = RootTransform.localPosition;
            _originalRotation = RootTransform.localEulerAngles;
            _originalScale = RootTransform.localScale;

            
            var rs = (isRandom) ? RandomUtils.RandomVector3(-strength, strength) : strength;
            tweener = typeShake switch
            {
                TypePunch.Position => RootTransform.DOPunchPosition(rs, settings.duration, vibrato, elasticity, snapping),
                TypePunch.Rotation => RootTransform.DOPunchRotation(rs,settings. duration, vibrato, elasticity),
                TypePunch.RotationBlend => RootTransform.DOBlendablePunchRotation(rs, settings.duration, vibrato, elasticity),
                TypePunch.Scale =>  RootTransform.DOPunchScale(rs, settings.duration, vibrato, elasticity),
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
            RootTransform.localPosition = _originalPosition;
            RootTransform.localEulerAngles = _originalRotation;
            RootTransform.localScale = _originalScale;
        }
    }
}