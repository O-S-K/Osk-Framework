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

        public override Tweener InitTween()
        {
            return isLocal ? transform.DOLocalMove(to, settings.duration, snapping) : transform.DOMove(to, settings.duration, snapping);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;

            if (isLocal)
                transform.localPosition = from;
            else
                transform.position = from;

            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType, settings.useUnscaledTime)
                .SetTarget(target)
                .SetRelative(negatives)
                .OnComplete(() => settings.eventCompleted?.Invoke());

            if (settings.typeAnim == TypeAnimation.Ease)
                tweener.SetEase(settings.ease);
            else
                tweener.SetEase(settings.curve);
        }

  
        public override void Stop()
        {
            base.Stop(); 
            if (isResetToFrom)
                if (isLocal)  transform.localPosition = from;
                else transform.position = from;
            else
                if (isLocal)  transform.localPosition = to;
                else  transform.position = to;
        }
    }
}