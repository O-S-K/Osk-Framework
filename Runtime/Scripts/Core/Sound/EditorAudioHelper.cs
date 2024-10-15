#if UNITY_EDITOR
using UnityEngine;

namespace OSK
{
    [ExecuteInEditMode]
    public static class EditorAudioHelper
    {
        private static AudioSource _editorAudioSource;

        public static void PlayClip(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioClip is null.");
                return;
            }

            // Create an AudioSource if it doesn't exist
            if (_editorAudioSource == null)
            {
                GameObject audioObject = new GameObject("EditorAudioSource");
                audioObject.hideFlags = HideFlags.HideAndDontSave;  // Hide the GameObject from hierarchy
                _editorAudioSource = audioObject.AddComponent<AudioSource>();
            }

            // Set the clip and play
            _editorAudioSource.clip = clip;
            _editorAudioSource.Play();
        } 
        
        public static void StopClip(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioClip is null.");
                return;
            }

            // Stop the clip
            _editorAudioSource.Stop();
        }
        
        // Check if a specific clip is currently playing
        public static bool IsClipPlaying(AudioClip clip)
        {
            return _editorAudioSource != null && _editorAudioSource.isPlaying && _editorAudioSource.clip == clip;
        }
    }
}
#endif