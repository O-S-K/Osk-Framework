using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class EffectProvider : DoTweenBaseProvider
    {
        public ParticleSystem particleSystem;
        public bool isSetActive;
        
        [HideInInspector] public bool from;
        [HideInInspector] public bool to;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

        public override void ProgressTween()
        {
            particleSystem.Stop();
            tweener = DOVirtual.Float(from ? 1 : 0, to ? 1 : 0, settings.duration, value =>
            {
                if(isSetActive)
                    particleSystem.gameObject.SetActive(true);
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
            particleSystem.Stop();
            if(isSetActive)
                particleSystem.gameObject.SetActive(false);
        }
    }
}