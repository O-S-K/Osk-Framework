using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class TransformPosProvider : DoTweenBaseProvider
    {
        public bool isLocal = true;
        public bool snapping = false;
        public Vector3 endValue = Vector3.zero;
        private void Reset() => endValue = isLocal ? transform.localPosition : transform.position;
        public override Tweener InitTween()
        {
            tweener = isLocal ? transform.DOLocalMove(endValue, duration, snapping) : transform.DOMove(endValue, duration, snapping);
            return tweener;
        }
        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
        }
    }
}