using DG.Tweening;

namespace OSK
{
    public interface IDoTweenProviderBehaviours
    {
        void InitFromMG(bool playOnEnable, bool setAutoKill, UpdateType updateType, bool useUnscaledTime);
        Tweener Tweener { get; }
        bool IsPlaying { get; }
        void ProgressTween();
        void Play();
        void Rewind();
        void Stop();
        float GetDuration();
        float GetCurrentDuration();
        void Preview(float time);
    }
}