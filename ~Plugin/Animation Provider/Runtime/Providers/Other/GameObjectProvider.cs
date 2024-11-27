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

        public override void ProgressTween()
        {
            gameObject.SetActive(from);
            tweener = DOVirtual.Float(from ? 1 : 0, to ? 1 : 0, settings.duration,
                value => gameObject.SetActive(value > 0));
            base.ProgressTween();
        }

       
        public override void Play()
        {
            base.Play();
        }


        public override void Stop()
        {
            base.Stop();
            gameObject.SetActive(from);
        }
    }
}