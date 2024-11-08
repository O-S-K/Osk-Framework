using DG.Tweening;

namespace OSK
{
    public interface IDoTweenProviderBehaviours
    {
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