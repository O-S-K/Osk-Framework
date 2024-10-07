using UnityEngine;

namespace OSK
{
    [System.Serializable]
    public class SoundInfo
    {
        public string id = "";
        public AudioClip audioClip = null;
        public SoundType type = SoundType.SoundEffect;

        [Range(0, 1)] public float clipVolume = 1;
    }


    public enum SoundType
    {
        SoundEffect,
        Music
    }
}