using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public class Appearer : MonoBehaviour
    {
        [Header("Appearer")] public Transform rootAppear;
        public float scaleInit = 0f;
        public bool ignoreTimeScale = true;

        [Header("Animation")] public Ease animStartButton = Ease.OutBack;
        public Ease animEndButton = Ease.InBack;
        public float duration = 0.3f;

        [Header("Delay")] public float appearAfter;
        public float hideDelay;

        private bool shown;
        private Transform root => rootAppear ? rootAppear : transform;
        private Tween tween;

        private void OnEnable()
        {
            root.localScale = Vector3.one * scaleInit;
            if (appearAfter <= 0)
                Show();
            else
                tween = DOVirtual.DelayedCall(appearAfter, Show).SetUpdate(ignoreTimeScale);
        }

        public void Show()
        {
            tween = root.DOScale(Vector3.one, duration).SetEase(animStartButton).SetUpdate(ignoreTimeScale);
            if (!shown)
            {
                shown = true;
            }
        }

        private void OnDisable()
        {
            tween.Kill();
            tween = null;
        }

        public void Hide()
        {
            tween = root.DOScale(Vector3.zero, duration).SetEase(animEndButton).SetUpdate(ignoreTimeScale);
            if (shown)
            {
                shown = false;
            }
        }

        public void HideWithDelay()
        {
            if (hideDelay <= 0)
                Hide();
            else
                tween = DOVirtual.DelayedCall(hideDelay, Hide).SetUpdate(ignoreTimeScale);
        }
    }
}