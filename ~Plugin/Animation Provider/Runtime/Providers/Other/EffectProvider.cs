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

        public override void ProgressTween()
        { 
            particleSystem.Stop();
            tweener =  DOVirtual.Float(form ? 1 : 0, to ? 1 : 0, settings.duration, value =>
            {
                //particleSystem.gameObject.SetActive(value > 0);
                if (value > 0)
                {
                    particleSystem.Play();
                } 
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
            tween?.Kill(); 
            particleSystem.Stop();
        }
    }
} 


