using System;
using CustomInspector;
using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public enum TransitionType
    {
        None,
        Fade,
        Scale,
        SlideRight,
        SlideLeft,
        SlideUp,
        SlideDown
    }

    [System.Serializable]
    public class TweenSettings
    {
        public TransitionType transition;

        [HideIf(nameof(transition), TransitionType.None)]
        public float time = 0.25f;

        [HideIf(nameof(transition), TransitionType.None), HideIf(nameof(useCustomCurve), true)]
        public bool useEase = false;

        [ShowIf(nameof(useEase))] public Ease ease = Ease.OutQuad;

        [ShowIf(nameof(transition), TransitionType.Scale)]
        public float initScale = 0;


        [HideIf(nameof(transition), TransitionType.None), HideIf(nameof(useEase), true)]
        public bool useCustomCurve = false;

        [ShowIf(nameof(useCustomCurve))] public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    
    
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class UITransition : MonoBehaviour
    {
        [Header("Content UI")] [SerializeField]
        private RectTransform contentUI;

        [SerializeField] private TweenSettings _openingTweenSettings;
        [SerializeField] private TweenSettings _closingTweenSettings;
        [Space(10)]

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        
        [Button]
        private void AddImageBlackFade()
        {
            var image = gameObject.GetOrAdd<UnityEngine.UI.Image>();
            image.color = new Color(0, 0, 0, 0.9f);
            image.rectTransform.anchorMin = Vector2.zero;
            image.rectTransform.anchorMax = Vector2.one;
            image.rectTransform.sizeDelta = Vector2.zero;
        }

        // Property to return either contentUI or _rectTransform
        private RectTransform TargetRectTransform => contentUI != null ? contentUI : _rectTransform;

        public void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OpenTrans(Action onComplete)
        {
            ResetTransitionState();

            if (_openingTweenSettings.transition == TransitionType.None)
            {
                onComplete();
                return;
            }

            Tween tween = null;

            switch (_openingTweenSettings.transition)
            {
                case TransitionType.Fade:
                    _canvasGroup.alpha = 0;
                    ApplyTween(_canvasGroup.DOFade(1, _openingTweenSettings.time), true);
                    break;

                case TransitionType.Scale:
                    TargetRectTransform.localScale = Vector3.one * _openingTweenSettings.initScale;
                    ApplyTween(TargetRectTransform.DOScale(Vector3.one, _openingTweenSettings.time), true);
                    break;

                case TransitionType.SlideRight:
                    SetAnchoredPosition(new Vector2(-TargetRectTransform.rect.width, 0));
                    ApplyTween(TargetRectTransform.DOAnchorPosX(0, _openingTweenSettings.time), true);
                    break;

                case TransitionType.SlideLeft:
                    SetAnchoredPosition(new Vector2(TargetRectTransform.rect.width, 0));
                    ApplyTween(TargetRectTransform.DOAnchorPosX(0, _openingTweenSettings.time), true);
                    break;

                case TransitionType.SlideUp:
                    SetAnchoredPosition(new Vector2(0, -TargetRectTransform.rect.height));
                    ApplyTween(TargetRectTransform.DOAnchorPosY(0, _openingTweenSettings.time), true);
                    break;

                case TransitionType.SlideDown:
                    SetAnchoredPosition(new Vector2(0, TargetRectTransform.rect.height));
                    ApplyTween(TargetRectTransform.DOAnchorPosY(0, _openingTweenSettings.time), true);
                    break;
            }

            tween?.OnComplete(() =>
            {
                ResetTransitionState();
                onComplete();
            });
        }

        private void SetAnchoredPosition(Vector2 position)
        {
            TargetRectTransform.anchoredPosition = position;
        }

        private void ApplyTween(Tween tween, bool isOpen)
        {
            if (isOpen)
            {
                if (_openingTweenSettings.useEase)
                {
                    tween.SetEase(_openingTweenSettings.ease);
                }
                else if (_openingTweenSettings.useCustomCurve)
                {
                    tween.SetEase(_openingTweenSettings.curve);
                }
                else
                {
                    tween.SetEase(Ease.Linear);
                }
            }
            else
            {
                if (_closingTweenSettings.useEase)
                {
                    tween.SetEase(_closingTweenSettings.ease);
                }
                else if (_closingTweenSettings.useCustomCurve)
                {
                    tween.SetEase(_closingTweenSettings.curve);
                }
                else
                {
                    tween.SetEase(Ease.Linear);
                }
            }
        }

        public void CloseTrans(Action onComplete)
        {
            ResetTransitionState();

            if (_closingTweenSettings.transition == TransitionType.None)
            {
                onComplete();
                return;
            }

            Tween tween = null;

            switch (_closingTweenSettings.transition)
            {
                case TransitionType.Fade:
                    tween = _canvasGroup.DOFade(0, _closingTweenSettings.time);
                    break;

                case TransitionType.Scale:
                    tween = TargetRectTransform.DOScale(Vector3.zero, _closingTweenSettings.time);
                    break;

                case TransitionType.SlideRight:
                    tween = TargetRectTransform.DOAnchorPosX(TargetRectTransform.rect.width,
                        _closingTweenSettings.time);
                    break;

                case TransitionType.SlideLeft:
                    tween = TargetRectTransform.DOAnchorPosX(-TargetRectTransform.rect.width,
                        _closingTweenSettings.time);
                    break;

                case TransitionType.SlideUp:
                    tween = TargetRectTransform.DOAnchorPosY(TargetRectTransform.rect.height,
                        _closingTweenSettings.time);
                    break;

                case TransitionType.SlideDown:
                    tween = TargetRectTransform.DOAnchorPosY(-TargetRectTransform.rect.height,
                        _closingTweenSettings.time);
                    break;
            }

            if (tween != null)
            {
                ApplyTween(tween, false);
                tween.OnComplete(() =>
                {
                    ResetTransitionState();
                    onComplete();
                });
            }
        }

        private void ResetTransitionState()
        {
            DOTween.Kill(TargetRectTransform);
            DOTween.Kill(_canvasGroup);

            _canvasGroup.alpha = 1;
            TargetRectTransform.localScale = Vector3.one;
            TargetRectTransform.anchoredPosition = Vector2.zero;
        }
    }
}