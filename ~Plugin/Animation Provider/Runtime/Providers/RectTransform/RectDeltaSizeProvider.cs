using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class RectDeltaSizeProvider : DoTweenBaseProvider
    {
        public bool snapping = false;
        public Vector3 to = Vector3.zero;
        public Vector3 from = Vector3.zero;
    
        
        public override void ProgressTween()
        {
            RootRectTransform.sizeDelta = from;
            target = RootRectTransform;
            tweener = RootRectTransform.DOSizeDelta(from,settings. duration, snapping);
            base.ProgressTween();
        }
 
        public override void Play()
        {
            base.Play();
        }


        public override void Stop()
        {
            base.Stop();
            RootRectTransform.sizeDelta = to;
        }
    }
}