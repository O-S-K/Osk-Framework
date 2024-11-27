using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

// When TMPro is centered, spaces will not move the entire string forward, so the entire string will jump back and forth. Unity will solve this problem.
//https://forum.unity.com/threads/why-there-is-no-setting-for-textmesh-pro-ugui-to-count-whitespace-at-the-end.676897/

namespace OSK
{
    [RequireComponent(typeof(Text))] 
    public class TextLoadingAnimationProvider : DoTweenBaseProvider
    { 
        [HideInInspector] public Text text;
        [HideInInspector] public int from = 0;
        [HideInInspector] public int to = 3;
        
        private string cached;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

        public string Text
        {
            get => text.text;
            set
            {
                text.text = value;
            }
        }

        public override void ProgressTween()
        {
            if (text)
            {
                string[] dots = { "   ", ".  ", ".. ", "...", };
                var msg = cached = Text;
                tweener = DOTween.To(() => from, v => Text = $"{msg}{dots[v]}", to, settings.duration);
                target = text;
            }
            else
            {
                Logg.LogError("Text is null");
            }

            base.ProgressTween();
        }


        public override void Play()
        {
            base.Play();
        }


        public override void Stop()
        {
            base.Stop();
            Text = cached;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (GetComponent<TMPNumScrollProvider>() != null)
            {
                Logg.LogError("TMPNumScrollProvider is already attached to this GameObject. Please remove it before adding TextLoadingAnimationProvider.");
                enabled = false;
            }
        }
#endif

    }
}