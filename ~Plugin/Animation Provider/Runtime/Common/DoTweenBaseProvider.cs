using UnityEngine;
using DG.Tweening;

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
        public Ease ease = Ease.Linear;
        public Tweener tweener;
        public Tweener Tweener => tweener;
        public bool IsPlaying => null != tweener && tweener.IsPlaying();

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
                .SetEase(ease)
                .SetLoops(loopcount, loopType)
                .SetTarget(target);
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