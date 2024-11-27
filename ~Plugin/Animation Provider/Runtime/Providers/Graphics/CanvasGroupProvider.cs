using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class CanvasGroupProvider : DoTweenBaseProvider
    {
        private CanvasGroup canvasGroup;
        public float from = 0;
        public float to = 1;

        public override void ProgressTween()
        {
            canvasGroup = gameObject.GetOrAdd<CanvasGroup>();
            canvasGroup.alpha = from;
            
            tweener = canvasGroup.DOFade(to, settings.duration);
            base.ProgressTween(); 
        }

  
        public override void Play()
        {
            base.Play();
        }


        public override void Stop()
        {
            base.Stop();
            canvasGroup.alpha = to;
        }
    }
}