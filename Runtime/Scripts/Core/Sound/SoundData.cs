using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class SoundData
    {
        public string id = "";
        public AudioClip audioClip = null;
        public SoundType type = SoundType.SFX;
        
        [Range(0, 1)] public float volume = 1;
        public string group = "Default";


#if UNITY_EDITOR
        public void Play()
        {
            if (audioClip == null)
            {
                Debug.LogWarning("AudioClip is null.");
                return;
            }
            EditorAudioHelper.PlayClip(audioClip);
        }

        public void Stop()
        {
            if (audioClip == null)
            {
                Debug.LogWarning("AudioClip is null.");
                return;
            }
            EditorAudioHelper.StopClip(audioClip);
        }
        
        public void SetVolume(float volume)
        {
            this.volume = volume;
            EditorAudioHelper.SetVolume(audioClip, volume);
        }

        public bool IsPlaying() => EditorAudioHelper.IsClipPlaying(audioClip);

        public void UpdateId()
        {
            id = audioClip != null ? audioClip.name : string.Empty;
        }
#endif
    }

    public enum SoundType
    {
        MUSIC = 0,    // Background music
        SFX = 1,      // Sound effects
        //AMBIENCE = 2, // Ambience sounds
        //VOICE = 3,   // Voice lines
    }
}