using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicProvider : DoTweenBaseProvider
    {
        public Color from = default;
        public Color to = default;
        public Graphic graphic;

        private void Reset()
        {
            graphic = graphic ? GetComponent<Graphic>() : graphic;
            to = graphic.color; // Capture the initial value
        }

        public override Tweener InitTween()
        {
            var arr = GetComponents<GraphicProvider>();
            var blendable = null != arr && arr.Length > 1;
            if (blendable)
            {
                Debug.Log($"Found multiple {nameof(GraphicProvider)} entering color mixing mode!");
            }

            return blendable ? graphic.DOBlendableColor(to, settings.duration) : graphic.DOColor(to, settings.duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;

            graphic = graphic ? GetComponent<Graphic>() : graphic;
            graphic.color = from;

            tweener = InitTween();
            tweener
                .SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType, settings.useUnscaledTime)
                .SetTarget(target)
                .OnComplete(() => settings.eventCompleted?.Invoke());

            if (settings.typeAnim == TypeAnimation.Ease)
                tweener.SetEase(settings.ease);
            else
                tweener.SetEase(settings.curve);
        }

        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //Reset the changes made by Dotween
            tweener = null;
            graphic.color = to;
        }
    }
}