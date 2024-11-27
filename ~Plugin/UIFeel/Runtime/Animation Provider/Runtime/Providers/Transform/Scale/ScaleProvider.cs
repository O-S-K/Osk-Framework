using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OSK
{
    public class ScaleProvider : DoTweenBaseProvider
    {
        [HideInInspector] public Vector3 from = Vector3.zero;
        [HideInInspector] public Vector3 to = Vector3.one;
        private Vector3 scaleCache;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;
 
        public override void ProgressTween()
        {
            scaleCache = RootTransform.localScale;
            RootTransform.localScale = from;
            tweener = RootTransform.DOScale(to, settings.duration);
            base.ProgressTween();
        }

        public override void Play()
        {
            base.Play();
        }

        public override void Stop()
        {
            base.Stop(); 
            RootTransform.localScale = scaleCache;
        }
    }
}