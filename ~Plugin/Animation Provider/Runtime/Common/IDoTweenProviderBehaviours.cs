using DG.Tweening;

namespace OSK
{
    public interface IDoTweenProviderBehaviours
    {
        TweenSettings settings { get; set; }
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