using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace OSK
{
    [System.Serializable]
    public class TweenSettings
    {
        public bool playOnEnable = true;
        public bool setAutoKill = true;

        public float delay = 0f;
        public float duration = 2f;
        public int loopcount = 0;
        public LoopType loopType = LoopType.Restart;
        public TypeAnimation typeAnim = TypeAnimation.Ease;
        public Ease ease = Ease.Linear;
        public AnimationCurve curve;
        public UpdateType updateType = UpdateType.Normal;
        public bool useUnscaledTime = false;
        public UnityEvent eventCompleted;
    }

    public abstract class DoTweenBaseProvider : MonoBehaviour, IDoTweenProviderBehaviours
    {
        [HideInInspector] public Object target;

        [SerializeField] private Transform _root;

        public Transform RootTransform => _root ? _root : transform;
        public RectTransform RootRectTransform => _root as RectTransform;


        public TweenSettings settings { get; set; } = new TweenSettings();

        public Tweener tweener;
        public Tweener Tweener => tweener;

        public bool IsPlaying => null != tweener && tweener.IsPlaying();

        public virtual void OnEnable()
        {
            if (settings.playOnEnable) Play();
        }

        public virtual void OnDisable() => Stop();

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
        }
#endif

        public abstract Tweener InitTween();
        public abstract void Play();

        public float Duration() => settings.duration;

        /*private void BaseInitTween()
        {
                tweener?.Kill();
                tweener = null;
                if (!target)
                    if (tweener != null)
                        target = (UnityEngine.Object)tweener.target;
                //
                tweener.SetDelay(delay)
                    .SetAutoKill(setAutoKill)
                    .SetLoops(loopcount, loopType)
                    .SetUpdate(isIgnoreTimeScale)
                    .SetTarget(target)
                    .OnComplete(() => onComplete?.Invoke());

                if (typeAnim == TypeAnimation.Ease)
                    tweener.SetEase(ease);
                else
                    tweener.SetEase(curve);
        }*/

        public void Preview(float time)
        {
            if (null == tweener) return;
            tweener.Goto(time);
        }

        public void PlayBackwards()
        {
            if (null == tweener) return;
            tweener.PlayBackwards();
        }

        public virtual void Rewind() => tweener?.Rewind();
        public virtual void Backward() => tweener?.PlayBackwards();
        public virtual void Stop() => tweener?.Kill();
        public virtual void Resume() => tweener?.Play();
        public virtual void Pause() => tweener?.Pause();
        public virtual void Kill() => tweener?.Kill();
        public virtual void Restart() => tweener?.Restart();
    }
}