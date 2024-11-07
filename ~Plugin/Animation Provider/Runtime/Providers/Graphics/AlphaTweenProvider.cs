using DG.Tweening;
using UnityEngine;
using CustomInspector;
using UnityEngine.Events;

namespace OSK
{
    public class AlphaTweenProvider : MonoBehaviour
    {
        [Range(0, 1), SerializeField] private float _alphaStart = 0.5f;
        [Range(0, 1), SerializeField] private float _alphaTarget = 0.5f;
        [Min(0), SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.Linear;

        [Min(0), SerializeField] private float _delay = 0f;
        [SerializeField] private bool _loop = false;

        [ShowIf(nameof(_loop), true)] [SerializeField]
        private LoopType _loopType = LoopType.Yoyo;

        [SerializeField] private bool ignoreTimeScale = true;

        [SerializeField] private UnityEvent _onComplete;
        [SerializeField] private UnityEngine.UI.Graphic _graphic;
        private Tween _tween;

        #if UNITY_EDITOR
        [Button]
        private void GetGraphic()
        {
            _graphic = gameObject.GetOrAdd<UnityEngine.UI.Graphic>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif

        private void OnEnable()
        {
            Play();
        }

        public void Play()
        {
            _graphic.DOFade(_alphaStart, 0);
            _tween = _graphic.DOFade(_alphaTarget, _duration)
                .SetDelay(_delay)
                .SetEase(_ease)
                .SetUpdate(ignoreTimeScale)
                .SetLoops(_loop ? -1 : 0, _loopType)
                .OnComplete(() => _onComplete?.Invoke());
        }

        public void Stop()
        {
            _tween.Kill();
            _graphic.DOFade(_alphaTarget, 0);
        }

        private void OnDisable()
        {
            _tween.Kill();
            _tween = null;
        }
    }
}