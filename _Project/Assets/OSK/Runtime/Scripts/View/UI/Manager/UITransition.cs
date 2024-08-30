using System;
using DG.Tweening;
using UnityEngine;

namespace OSK
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class UITransition : MonoBehaviour
    {
        public enum TransitionType
        {
            None,
            Fade,
            Zoom,
            SlideRight,
            SlideLeft,
            SlideUp,
            SlideDown
        }

        [System.Serializable]
        public class TweenSettings
        {
            public float duration = 0.5f;
            public Ease ease = Ease.OutQuad;
        }

        [SerializeField] private TransitionType _openingTransition;
        [SerializeField] private TransitionType _closingTransition;
        
        [SerializeField] private TweenSettings _openingTweenSettings;
        [SerializeField] private TweenSettings _closingTweenSettings;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void PlayOpeningTransition(Action onComplete)
        {
            ResetTransitionState();

            if (_openingTransition == TransitionType.None)
            {
                onComplete();
                return;
            }

            Tween tween = null;

            switch (_openingTransition)
            {
                case TransitionType.Fade:
                    _canvasGroup.alpha = 0;
                    tween = _canvasGroup.DOFade(1, _openingTweenSettings.duration)
                        .SetEase(_openingTweenSettings.ease);
                    break;

                case TransitionType.Zoom:
                    _rectTransform.localScale = Vector3.zero;
                    tween = _rectTransform.DOScale(Vector3.one, _openingTweenSettings.duration)
                        .SetEase(_openingTweenSettings.ease);
                    break;

                case TransitionType.SlideRight:
                    _rectTransform.anchoredPosition = new Vector2(-_rectTransform.rect.width, 0);
                    tween = _rectTransform.DOAnchorPosX(0, _openingTweenSettings.duration)
                        .SetEase(_openingTweenSettings.ease);
                    break;

                case TransitionType.SlideLeft:
                    _rectTransform.anchoredPosition = new Vector2(_rectTransform.rect.width, 0);
                    tween = _rectTransform.DOAnchorPosX(0, _openingTweenSettings.duration)
                        .SetEase(_openingTweenSettings.ease);
                    break;

                case TransitionType.SlideUp:
                    _rectTransform.anchoredPosition = new Vector2(0, -_rectTransform.rect.height);
                    tween = _rectTransform.DOAnchorPosY(0, _openingTweenSettings.duration)
                        .SetEase(_openingTweenSettings.ease);
                    break;

                case TransitionType.SlideDown:
                    _rectTransform.anchoredPosition = new Vector2(0, _rectTransform.rect.height);
                    tween = _rectTransform.DOAnchorPosY(0, _openingTweenSettings.duration)
                        .SetEase(_openingTweenSettings.ease);
                    break;
            }

            tween.OnComplete(() =>
            {
                ResetTransitionState();
                onComplete();
            });
        }

        public void PlayClosingTransition(Action onComplete)
        {
            ResetTransitionState();

            if (_closingTransition == TransitionType.None)
            {
                onComplete();
                return;
            }

            bool isCompleted = false;

            switch (_closingTransition)
            {
                case TransitionType.Fade:
                    _canvasGroup.DOFade(0, _closingTweenSettings.duration)
                        .SetEase(_closingTweenSettings.ease).OnComplete(() =>
                        {
                            ResetTransitionState();
                            onComplete();
                        });
                    break;
                case TransitionType.Zoom:
                    _rectTransform.DOScale(Vector3.zero, _closingTweenSettings.duration)
                        .SetEase(_closingTweenSettings.ease).OnComplete(() =>
                        {
                            ResetTransitionState();
                            onComplete();
                        });
                    break;
                case TransitionType.SlideRight:
                    _rectTransform.DOAnchorPosX(_rectTransform.rect.width, _closingTweenSettings.duration)
                        .SetEase(_closingTweenSettings.ease).OnComplete(() =>
                        {
                            ResetTransitionState();
                            onComplete();
                        });
                    break;
                case TransitionType.SlideLeft:
                    _rectTransform.DOAnchorPosX(-_rectTransform.rect.width, _closingTweenSettings.duration)
                        .SetEase(_closingTweenSettings.ease).OnComplete(() =>
                        {
                            ResetTransitionState();
                            onComplete();
                        });
                    break;
                case TransitionType.SlideUp:
                    _rectTransform.DOAnchorPosY(_rectTransform.rect.height, _closingTweenSettings.duration)
                        .SetEase(_closingTweenSettings.ease).OnComplete(() => isCompleted = true);
                    break;
                case TransitionType.SlideDown:
                    _rectTransform.DOAnchorPosY(-_rectTransform.rect.height, _closingTweenSettings.duration)
                        .SetEase(_closingTweenSettings.ease).OnComplete(() =>
                        {
                            ResetTransitionState();
                            onComplete();
                        });
                    break;
            }
        }

        private void ResetTransitionState()
        {
            DOTween.Kill(_rectTransform);
            DOTween.Kill(_canvasGroup);

            _canvasGroup.alpha = 1;
            _rectTransform.localScale = Vector3.one;
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}