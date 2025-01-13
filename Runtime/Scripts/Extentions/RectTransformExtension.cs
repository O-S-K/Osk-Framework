using UnityEngine;
using DG.Tweening;

namespace OSK
{
    public static class RectTransformExtension
    {
        public static Tweener DOResize(this RectTransform rectTransform, Vector2 endValue, float duration, bool snapping = false)
        {
            return rectTransform.DOSizeDelta(endValue, duration, snapping);
        }
    }
}