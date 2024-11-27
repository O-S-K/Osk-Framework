using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace OSK
{
    [System.Serializable]
    public class TweenSettings
    {
        public bool playOnEnable = true;
        public bool setAutoKill = true;

        [Min(0)]
        public float delay = 0f;
        
        [Min(0)]
        public float duration = 2f;
        
        [Min(-1)]
        public int loopcount = 0;
        
        [ShowIf("@loopcount == -1")] 
        public LoopType loopType = LoopType.Restart;
        
        public TypeAnimation typeAnim = TypeAnimation.Ease;
        
        [ShowIf(nameof(typeAnim), TypeAnimation.Ease)]
        public Ease ease = Ease.Linear;
        
        [ShowIf(nameof(typeAnim), TypeAnimation.Curve)]
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


        public TweenSettings settings = new TweenSettings();

        public Tweener tweener;
        public Tweener Tweener => tweener;

        public bool IsPlaying => null != tweener && tweener.IsPlaying();

        public virtual void OnEnable()
        {
            if (settings.playOnEnable)
            {
                Play();
            }
        }

        public virtual void OnDisable()
        {
            Stop();
        }

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
        }
#endif

        public virtual void SetInit(bool playOnEnable, bool setAutoKill, float delay, UpdateType updateType,
            bool useUnscaledTime)
        {
            settings.playOnEnable = playOnEnable;
            settings.setAutoKill = setAutoKill;
            settings.delay += delay;
            settings.updateType = updateType;
            settings.useUnscaledTime = useUnscaledTime;
        }

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

        public virtual void Stop()
        {
            tweener?.Kill();
            tweener = null;
        }
        public virtual void Resume() => tweener?.Play();
        public virtual void Pause() => tweener?.Pause();
        public virtual void Kill() => tweener?.Kill();
        public virtual void Restart() => tweener?.Restart();
    }
}