using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace OSK
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupProvider : DoTweenBaseProvider
    {
        [HideInInspector] public CanvasGroup canvasGroup;
        [HideInInspector] public float from = 0;
        [HideInInspector] public float to = 1;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

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