using System;
using CustomInspector;
using DG.Tweening;
using UnityEngine; 

namespace OSK
{
    public class GameObjectProvider : DoTweenBaseProvider
    {
        public GameObject gameObject;
        public bool startValue;
        public bool endValue;
        
        private Tween tween;

        public override Tweener InitTween()
        {
           return  DOVirtual.Float(startValue ? 1 : 0, endValue ? 1 : 0, duration, value => gameObject.SetActive(value > 0));
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            gameObject.SetActive(startValue);
            
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
            tweener = InitTween();
            tweener.SetDelay(delay)
                .SetAutoKill(setAutoKill) 
                .SetUpdate(isIgnoreTimeScale)
                .SetTarget(target)
                .OnComplete(DoneTween); 
        }

        private void DoneTween()
        {
                Debug.Log($"onComplete has listeners: {onComplete.GetPersistentEventCount()}");
                onComplete?.Invoke();
        }
 
        public override void Stop()
        {
            base.Stop();
            tween?.Kill(); 
            gameObject.SetActive(startValue);
        }
    }
} 
