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
        
        public override void ProgressTween()
        { 
            target =  text;
            text.text = form.ToString();
            tweener = DOTween.To(() => 0, y => text.text = y.ToString(), to, settings.duration);
            base.ProgressTween();
        }

       
        public override void Play()
        {
            base.Play();
        }


        private void Reset()
        {
            tweener?.Kill();
            tweener = null;
            text.text = form.ToString();
        }
    }
}