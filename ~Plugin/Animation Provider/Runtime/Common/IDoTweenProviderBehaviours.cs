using DG.Tweening;

namespace OSK
{
    public interface IDoTweenProviderBehaviours
    {
        void SetInit(bool playOnEnable, bool setAutoKill, float delay, UpdateType updateType, bool useUnscaledTime);
        Tweener Tweener { get; }
        bool IsPlaying { get; }
        Tweener InitTween();
        void Play();
        void Rewind();
        void Stop();
        float Duration();
        void Preview(float time);
    }
}