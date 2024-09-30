using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public class Appearer : MonoBehaviour
    {
        [Header("Appearer")]
        public Transform rootAppear;
        public float scaleInit = 0f;
        
        [Header("Animation")]
        public Ease animStartButton = Ease.OutBack;
        public Ease animEndButton = Ease.InBack;
        public float duration = 0.3f;

        [Header("Delay")]
        public float appearAfter;
        public float hideDelay;

        private bool shown;
        private Transform root => rootAppear ? rootAppear : transform;

        private void OnEnable()
        {
            root.localScale = Vector3.one * scaleInit;
            if (appearAfter >= 0)
                Invoke(nameof(Show), appearAfter);
        }

        public void Show()
        {
            root.DOScale(Vector3.one, duration).SetEase(animStartButton).SetUpdate(true);
            if (!shown)
            {
                shown = true;
            }
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(Show));
            CancelInvoke(nameof(Hide));
        }

        public void Hide()
        {
            root.DOScale(Vector3.zero, duration).SetEase(animEndButton).SetUpdate(true);
            if (shown)
            {
                shown = false;
            }
        }

        public void HideWithDelay()
        {
            Invoke(nameof(Hide), hideDelay);
        }
    }
}