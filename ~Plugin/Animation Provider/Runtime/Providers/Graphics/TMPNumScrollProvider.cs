using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OSK
{
    [DisallowMultipleComponent]
    public class TMPNumScrollProvider : DoTweenBaseProvider
    {
        public Text text;
        public int form;
        public int to;
        
        public override Tweener InitTween()
        { 
            target =  text;
            return DOTween.To(() => 0, y => text.text = y.ToString(), to, settings.duration); //tostring high GC
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;     
            
            text.text = form.ToString();
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
            text.text = form.ToString();
        }
    }
}