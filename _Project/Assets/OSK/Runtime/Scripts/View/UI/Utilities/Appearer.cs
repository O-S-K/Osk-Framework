using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public class Appearer : MonoBehaviour
    {
        public Ease animStartButton = Ease.OutBack;
        public Ease animEndButton = Ease.InBack;
        public float duration = 0.3f;

        public float appearAfter;
        public float hideDelay;

        private bool shown;

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
            if (appearAfter >= 0)
                Invoke(nameof(Show), appearAfter);
        }

        public void Show()
        {
            transform.DOScale(Vector3.one, duration).SetEase(animStartButton);
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
            transform.DOScale(Vector3.zero, duration).SetEase(animEndButton);
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