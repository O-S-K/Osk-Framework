using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class EffectProvider : DoTweenBaseProvider
    {
        public ParticleSystem  particleSystem;
        public bool form;
        public bool to;
        
        private Tween tween;

        public override Tweener InitTween()
        {
            return  DOVirtual.Float(form ? 1 : 0, to ? 1 : 0, settings.duration, value =>
            {
                //particleSystem.gameObject.SetActive(value > 0);
                if (value > 0)
                {
                    particleSystem.Play();
                } 
            });
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            particleSystem.Stop();
            
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill) 
                .SetUpdate(settings.updateType, settings.useUnscaledTime)
                .SetTarget(target)
                .OnComplete( () => settings.eventCompleted?.Invoke());
        }
 
 
        public override void Stop()
        {
            base.Stop();
            tween?.Kill(); 
            particleSystem.Stop();
        }
    }
} 


