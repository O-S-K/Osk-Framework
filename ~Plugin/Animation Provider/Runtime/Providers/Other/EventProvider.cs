using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace OSK
{
    public class EventProvider : DoTweenBaseProvider
    {
        public UnityEvent onComplete;
        public bool from;
        public bool to;

        private Tween tween;

        public override void ProgressTween()
        { 
            onComplete.RemoveAllListeners();
            tweener = DOVirtual.Float(from ? 1 : 0, to ? 1 : 0, settings.duration, value =>
            {
                if (value > 0)
                    onComplete?.Invoke();
            });
            
            base.ProgressTween();
        }

        
        public override void Play()
        {
            base.Play();
        }


        public override void Stop()
        {
            base.Stop();
            onComplete.RemoveAllListeners();
        }
    }
}