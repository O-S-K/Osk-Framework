using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class SoundInfo
    {
        public string id = "";
        public AudioClip audioClip = null;
        public SoundType type = SoundType.SFX;
        [Range(0, 1)] public float clipVolume = 1;

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

        public bool IsPlaying()
        {
            return EditorAudioHelper.IsClipPlaying(audioClip);
        }

        public void UpdateId()
        {
            if (audioClip != null)
            {
                id = audioClip.name;
            }
            else
            {
                id = string.Empty;
            }
        }

#endif
    }

    public enum SoundType
    {
        Music = 0,
        SFX = 1
    }
}