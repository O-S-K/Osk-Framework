using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace OSK
{
    [DisallowMultipleComponent]
    public class PositionProvider : DoTweenBaseProvider
    {
        public bool isLocal = true;
        public bool snapping = false;
        public bool negatives = false;
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.zero;

        public bool isResetToFrom = false;


        [ContextMenu("Get From")]
        public void GetPositionFrom() => from = isLocal ? transform.localPosition : transform.position;

        [ContextMenu("Get To")]
        public void GetPositionTo() => to = isLocal ? transform.localPosition : transform.position;

        public override void ProgressTween()
        {
            if (isLocal)
                transform.localPosition = from;
            else
                transform.position = from;
            
            tweener = isLocal
                ? transform.DOLocalMove(to, settings.duration, snapping)
                : transform.DOMove(to, settings.duration, snapping);
            tweener.SetRelative(negatives);
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