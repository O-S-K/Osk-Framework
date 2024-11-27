using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectDeltaSizeProvider : DoTweenBaseProvider
    {
        [HideInInspector] public Vector3 to = Vector3.zero;
        [HideInInspector] public Vector3 from = Vector3.zero;
        
        public override object GetStartValue() => from;
        public override object GetEndValue() => to;
        
        public override void ProgressTween()
        {
            RootRectTransform.sizeDelta = from;
            target = RootRectTransform;
            tweener = RootRectTransform.DOSizeDelta(from,settings. duration);
            base.ProgressTween();
        }
 
        public override void Play()
        {
            base.Play();
        }
 
        public override void Stop()
        {
            base.Stop();
            RootRectTransform.sizeDelta = from;
        }
    }
}