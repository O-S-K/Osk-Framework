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
        
        public override void ProgressTween()
        { 
            target =  image;
            image.fillAmount = from;
            tweener = DOTween.To(() => 0, y => image.fillAmount = (float)y, to, settings. duration);
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
            image.fillAmount = from;
        }
    }
}