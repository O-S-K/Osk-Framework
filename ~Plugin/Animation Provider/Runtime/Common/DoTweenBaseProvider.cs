using CustomInspector;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace OSK
{
    public abstract class DoTweenBaseProvider : MonoBehaviour, IDoTweenProviderBehaviours
    {
        [HideInInspector] public Object target;

        [Tooltip("When checked, the animation will start playing every time the game object is activated")]
        public bool playOnAwake = false;

        [Tooltip(
            "When checked, the animation is destroyed upon completion. If you need to rewind, please set it to false")]
        public bool setAutoKill = true;

        public float delay = 0f;
        public float duration = 2f;
        public int loopcount = 0;
        public LoopType loopType = LoopType.Restart;
        
        public TypeAnimation typeAnim = TypeAnimation.Ease;
 
        [HideIf(nameof(typeAnim), TypeAnimation.Curve)]
        public Ease ease = Ease.Linear;
        [HideIf(nameof(curve), TypeAnimation.Ease)]
        public AnimationCurve curve;
        
        public bool isIgnoreTimeScale = true;
        
        public Tweener tweener;
        public Tweener Tweener => tweener;
        public bool IsPlaying => null != tweener && tweener.IsPlaying();
        
        public UnityEvent onComplete;

        public virtual void OnEnable()
        {
            if (playOnAwake) Play();
        }

        public virtual void OnDisable() => Stop();

        public virtual void OnValidate()
        {
        }

        public virtual void Play()
        {
            tweener?.Kill();
            tweener = null;
            tweener = InitTween();
            if (!target) target = (Object)tweener.target;
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
        }

        public abstract Tweener InitTween();
        public virtual void Stop() => tweener?.Kill();

        public float GetDuration()
        { 
            return duration;
        }

        public void Preview(float time)
        {
            if (null == tweener) return;
            tweener.Goto(time);
        }

        public virtual void Rewind() => tweener?.Rewind();
    }
}