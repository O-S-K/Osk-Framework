using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    [DisallowMultipleComponent]
    public class FillAmountProvider : DoTweenBaseProvider
    {
        public Image image;
        [Range(0, 1)]
        public float from;
        
        [Range(0, 1)]
        public float to;
        
        public override Tweener InitTween()
        { 
            target =  image;
            return DOTween.To(() => 0, y => image.fillAmount = (float)y, to, settings. duration);
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;     
            
            image.fillAmount = from;
            tweener = InitTween();
            tweener.SetDelay(settings.delay)
                .SetAutoKill(settings.setAutoKill)
                .SetLoops(settings.loopcount, settings.loopType)
                .SetUpdate(settings.updateType, settings.useUnscaledTime)
                .SetTarget(target)
                .OnComplete(() => settings.eventCompleted?.Invoke());

            if (settings.typeAnim == TypeAnimation.Ease)
                tweener.SetEase(settings.ease);
            else
                tweener.SetEase(settings.curve);
        }

        private void Reset()
        {
            tweener?.Kill();
            tweener = null;
            image.fillAmount = from;
        }
    }
}