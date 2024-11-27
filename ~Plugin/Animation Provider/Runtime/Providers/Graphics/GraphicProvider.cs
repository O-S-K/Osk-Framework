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
 

        public override void ProgressTween()
        { 
            graphic = graphic ? GetComponent<Graphic>() : graphic;
            graphic.color = from;

            var arr = GetComponents<GraphicProvider>();
            var blendable = null != arr && arr.Length > 1;
            if (blendable)
            {
                Debug.Log($"Found multiple {nameof(GraphicProvider)} entering color mixing mode!");
            }

            tweener = blendable ? graphic.DOBlendableColor(to, settings.duration) : graphic.DOColor(to, settings.duration);
        }

  
        public override void Play()
        {
            base.Play();
        }


        public override void Stop()
        {
            base.Stop();
            graphic.color = to;
        }
    }
}