using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

// When TMPro is centered, spaces will not move the entire string forward, so the entire string will jump back and forth. Unity will solve this problem.
//https://forum.unity.com/threads/why-there-is-no-setting-for-textmesh-pro-ugui-to-count-whitespace-at-the-end.676897/

namespace OSK
{
    [RequireComponent(typeof(Graphic))]
    public class TextLoadingAnimationProvider : DoTweenBaseProvider
    {
        [Header("For Text  and TextMeshPro")] public Graphic text;
        private string cached;

        public string Text
        {
            get => text is Text ? (text as Text).text : (text as TextMeshProUGUI).text;
            set
            {
                if (text is Text)
                {
                    (text as Text).text = value;
                }
                else
                {
                    (text as TextMeshProUGUI).text = value;
                }
            }
        }

        public override Tweener InitTween()
        {
            if (text)
            {
                string[] dots = { "   ", ".  ", ".. ", "...", };
                var msg = cached = Text;
                tweener = DOTween.To(() => 0, v => Text = $"{msg}{dots[v]}", 3, settings. duration);
                target = text; // This must be written!
            }
            else
            {
                Debug.LogError(
                    $"{nameof(TextLoadingAnimationProvider)}:  The Text Loading component requires a Text or TMP component!");
            }

            return tweener;
        }

        public override void Play()
        {
            tweener?.Kill();
            tweener = null;
            if (!target)
                if (tweener != null)
                    target = (UnityEngine.Object)tweener.target;
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

        public override void Stop()
        {
            base.Stop();
            tweener = null;
            Text = cached;
        }

        public override void OnValidate()
        {
            base.OnValidate();
            if (!(text is Text) && !(text is TMPro.TextMeshProUGUI))
            {
                Debug.LogError(
                    $"{nameof(TextLoadingAnimationProvider)}: The Text Loading component requires a Text or TMP component!");
                text = null;
            }
        }
    }
}