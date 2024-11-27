using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine; 

namespace OSK
{
    public class GameObjectProvider : DoTweenBaseProvider
    {
        public GameObject gameObject;
        public bool from;
        public bool to;
        
        public override Tweener InitTween()
        {
           return  DOVirtual.Float(from ? 1 : 0, to ? 1 : 0, settings.duration, value => gameObject.SetActive(value > 0));
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            gameObject.SetActive(from);
            
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
            gameObject.SetActive(from);
        }
    }
} 
