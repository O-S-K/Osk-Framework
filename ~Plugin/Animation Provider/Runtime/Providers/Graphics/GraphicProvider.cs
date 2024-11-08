using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicProvider : DoTweenBaseProvider
    {
        public Color startValue = default;
        public Color endValue = default;
        public Graphic graphic;

        private void Reset()
        {
            graphic = graphic ? GetComponent<Graphic>() : graphic;
            endValue = graphic.color; // Capture the initial value
        }

        public override Tweener InitTween()
        {
            var arr = GetComponents<GraphicProvider>();
            var blendable = null != arr && arr.Length > 1;
            if (blendable)
            {
                Debug.Log($"Found multiple {nameof(GraphicProvider)} entering color mixing mode!");
            }
            return blendable ? graphic.DOBlendableColor(endValue, duration) : graphic.DOColor(endValue, duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;

            graphic = graphic ? GetComponent<Graphic>() : graphic;
            graphic.color = startValue;

            tweener = InitTween();
            tweener
                .SetDelay(delay)
                .SetAutoKill(setAutoKill)
                .SetLoops(loopcount, loopType)
                .SetUpdate(isIgnoreTimeScale)
                .SetTarget(target)
                .OnComplete(() => onComplete?.Invoke());

            if (typeAnim == TypeAnimation.Ease)
                tweener.SetEase(ease);
            else
                tweener.SetEase(curve);
        }

        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
            graphic.color = endValue;
        }
    }
}