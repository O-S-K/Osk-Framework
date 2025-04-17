using UnityEngine;

namespace OSK
{
    public class PlayingSound
    {
        public SoundData SoundData = null;
        public AudioSource AudioSource = null;
        public bool IsPaused = false;
        public bool IsPlaying => AudioSource.isPlaying;
    }
}