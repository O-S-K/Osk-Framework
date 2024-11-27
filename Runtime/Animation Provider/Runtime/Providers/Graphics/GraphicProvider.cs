using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    public class GraphicProvider : DoTweenBaseProvider
    {
        [HideInInspector] public Graphic graphic;
        [HideInInspector] public Color from = default;
        [HideInInspector] public Color to = default;

        private Color startColor;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

        public override void ProgressTween()
        {
            graphic = graphic ? GetComponent<Graphic>() : graphic;
            startColor = graphic.color;
            graphic.color = from;

            var arr = GetComponents<GraphicProvider>();
            var blendable = null != arr && arr.Length > 1;
            if (blendable)
            {
                Logg.Log($"Found multiple {nameof(GraphicProvider)} entering color mixing mode!");
            }

            tweener = blendable
                ? graphic.DOBlendableColor(to, settings.duration)
                : graphic.DOColor(to, settings.duration);
        }

        public override void Play()
        {
            base.Play();
        }

        public override void Stop()
        {
            base.Stop();
            graphic.color =  startColor;
        }
    }
}