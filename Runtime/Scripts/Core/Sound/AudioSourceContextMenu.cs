#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OSK
{
    public class AudioSourceContextMenu 
    {
        [MenuItem("CONTEXT/AudioSource/▶ Play")]
        private static void Play(MenuCommand command)
        {
            AudioSource audioSource = (AudioSource)command.context;
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }

        [MenuItem("CONTEXT/AudioSource/❚❚ Pause")]
        private static void Pause(MenuCommand command)
        {
            AudioSource audioSource = (AudioSource)command.context;
            audioSource.Pause();
        }

        [MenuItem("CONTEXT/AudioSource/⏯ UnPause")]
        private static void UnPause(MenuCommand command)
        {
            AudioSource audioSource = (AudioSource)command.context;
            audioSource.UnPause();
        }

        [MenuItem("CONTEXT/AudioSource/■ Stop")]
        private static void Stop(MenuCommand command)
        {
            AudioSource audioSource = (AudioSource)command.context;
            audioSource.Stop();
        }
    }
}

#endif