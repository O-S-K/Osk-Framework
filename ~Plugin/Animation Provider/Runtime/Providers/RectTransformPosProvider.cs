using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class RectTransformPosProvider : DoTweenBaseProvider
    {
        public bool snapping = false;
        public Vector3 endValue = Vector3.zero;
        public RectTransform rectTransform;
        private RectTransform _rectTransform => rectTransform ? rectTransform : GetComponent<RectTransform>();

        private void Reset() => endValue = _rectTransform.anchoredPosition;

        public override Tweener InitTween()
        {
            tweener = _rectTransform.DOAnchorPos(endValue, duration, snapping);
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