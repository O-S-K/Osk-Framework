using System;
using UnityEngine;
using DG.Tweening;

namespace OSK
{
    [DisallowMultipleComponent]
    public class RectPositionProvider : DoTweenBaseProvider
    {
        [HideInInspector] public Vector3 from = Vector3.zero;
        [HideInInspector] public Vector3 to = Vector3.zero;

        public override object GetStartValue() => from;
        public override object GetEndValue() => to;

        public override void ProgressTween()
        {
            RootRectTransform.anchoredPosition = from;
            tweener = RootRectTransform.DOAnchorPos(to, settings.duration);
            base.ProgressTween();
        }

        public override void Play()
        {
            base.Play();
        }

        public override void Stop()
        {
            base.Stop();
            RootRectTransform.anchoredPosition = to;
        }
    }
}