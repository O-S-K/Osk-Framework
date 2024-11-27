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
        [Header("Appearer")] 
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.one;

        [Header("Hide")] 
        public Ease animEndButton = Ease.InBack;
        public float hideDelay;

        private Tween tween;

        public override void ProgressTween()
        {
            RootTransform.localScale = from;
           tweener = RootTransform.DOScale(to, settings.duration);
        }

  
        public override void Play()
        {
            base.Play();
        }


        public void Hide()
        {
            tweener = RootTransform.DOScale(Vector3.zero, settings.duration)
                .SetEase(animEndButton)
                .OnComplete(() =>
                { 
                    gameObject.SetActive(false);
                });
        }

        public override void Stop()
        {
            base.Stop();
            tween?.Kill(); 
            RootTransform.localScale = to;
        }

        [Button]
        public void HideWithDelay()
        {
            if (hideDelay <= 0)
                Hide();
            else
                tween = DOVirtual.DelayedCall(hideDelay, Hide);
        }
    }
}