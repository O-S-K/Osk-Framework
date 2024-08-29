using UnityEngine;

public class StepPitch : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;
    
    private float _step;
    private float _progress;

    public void Initialize(int steps)
    {
        _step = 1f / steps;
        _progress = 0;
    }

    public void PlayStep()
    {
        if (_progress < 1f)
        {
            _audioSource.pitch = 0.5f + (_progress * 0.5f);
            _audioSource.PlayOneShot(_clip);
            _progress += _step;
        }
    }
}
