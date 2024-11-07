using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectTransformDeltaSizeProvider : DoTweenBaseProvider
    {
        public bool snapping = false;
        public Vector3 endValue = Vector3.zero;
        [HideInInspector]
        public RectTransform rectTransform => transform as RectTransform;
        private void Reset() => endValue = (transform as RectTransform).sizeDelta;
        public override Tweener InitTween()
        {
            target = rectTransform;
            return rectTransform.DOSizeDelta(endValue, duration, snapping);
        }

        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
        }
    }
}