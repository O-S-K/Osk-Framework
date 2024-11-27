using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class PositionProvider : DoTweenBaseProvider
    {
        [HideInInspector] public Vector3 from = Vector3.zero;
        [HideInInspector] public Vector3 to = Vector3.zero;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

        
        public bool isResetToFrom = false;
        public bool isLocal = true; 
        
        public override void ProgressTween()
        {
            if (isLocal)
                transform.localPosition = from;
            else
                transform.position = from;

            tweener = isLocal
                ? transform.DOLocalMove(to, settings.duration)
                : transform.DOMove(to, settings.duration);;
            base.ProgressTween();
        }

        public override void Play()
        {
            base.Play();
        }

        public override void Stop()
        {
            base.Stop();
            if (isResetToFrom)
                if (isLocal) transform.localPosition = from;
                else transform.position = from;
            else if (isLocal) transform.localPosition = to;
            else transform.position = to;
        }
    }
}