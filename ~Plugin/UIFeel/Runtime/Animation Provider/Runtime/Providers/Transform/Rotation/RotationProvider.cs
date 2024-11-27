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

        [HideInInspector]
        public Vector3 from = Vector3.zero;
        [HideInInspector]
        public Vector3 to = Vector3.zero;
        
        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

        public override void ProgressTween()
        { 
            RootTransform.localEulerAngles = from;
            tweener = isLocal
                ? RootTransform.DOLocalRotate(to, settings.duration, rotateMode)
                : RootTransform.DORotate(to, settings.duration, rotateMode);
            
            base.ProgressTween();
        }

 
        public override void Play()
        {
            base.Play();
        }

        public override void Stop()
        {
            base.Stop(); 
            RootTransform.localEulerAngles = from;
        }
    }
}