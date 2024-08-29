using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class SoundManager : GameFrameworkComponent
    {
        [System.Serializable]
        public class SoundInfo
        {
            public string id = "";
            public AudioClip audioClip = null;
            public SoundType type = SoundType.SoundEffect;
            public float clipVolume = 1;
            public float pitch = 1;
        }

        private class PlayingSound
        {
            public SoundInfo soundInfo = null;
            public AudioSource audioSource = null;
        }

        public enum SoundType
        {
            SoundEffect,
            Music
        }

        public List<SoundInfo> soundInfos = null;
        private List<PlayingSound> MusicInfos;

        public bool IsMusicOn { get; private set; }
        public bool IsSoundEffectsOn { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            MusicInfos = new List<PlayingSound>();

            IsMusicOn = true;
            IsSoundEffectsOn = true;
        }


        private void Update()
        {
            if (MusicInfos == null)
                return;

            for (int i = 0; i < MusicInfos.Count; i++)
            {
                AudioSource audioSource = MusicInfos[i].audioSource;

                // If the Audio Source is no longer playing then return it to the pool so it can be re-used
                if (!audioSource.isPlaying)
                {
                    Destroy(audioSource.gameObject);
                    MusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Plays the sound with the give id
        /// </summary>
        public void Play(string id, bool loop)
        {
            Play(id, loop, 0);
        }

        /// <summary>
        /// Plays the sound with the give id, if loop is set to true then the sound will only stop if the Stop method is called
        /// </summary>
        public void Play(string id, bool loop, float playDelay)
        {
            SoundInfo soundInfo = GetSoundInfo(id);

            if (soundInfo == null)
            {
                Debug.LogError("[SoundManager] There is no Sound Info with the given id: " + id);

                return;
            }

            if ((soundInfo.type == SoundType.Music && !IsMusicOn) ||
                (soundInfo.type == SoundType.SoundEffect && !IsSoundEffectsOn))
            {
                return;
            }

            AudioSource audioSource = CreateAudioSource(id);

            audioSource.clip = soundInfo.audioClip;
            audioSource.loop = loop;
            audioSource.time = 0;
            audioSource.volume = soundInfo.clipVolume;
            audioSource.pitch = soundInfo.pitch;

            if (playDelay > 0)
            {
                audioSource.PlayDelayed(playDelay);
            }
            else
            {
                audioSource.Play();
            }

            PlayingSound playingSound = new PlayingSound();

            playingSound.soundInfo = soundInfo;
            playingSound.audioSource = audioSource;

            MusicInfos.Add(playingSound);
        }

        /// <summary>
        /// Stops all playing sounds with the given id
        /// </summary>
        public void Stop(string id)
        {
            StopAllSounds(id, MusicInfos);
        }

        /// <summary>
        /// Stops all playing sounds with the given type
        /// </summary>
        public void Stop(SoundType type)
        {
            StopAllSounds(type, MusicInfos);
        }

        /// <summary>
        /// Sets the SoundType on/off
        /// </summary>
        public void SetSoundTypeOnOff(SoundType type, bool isOn)
        {
            switch (type)
            {
                case SoundType.SoundEffect:

                    if (isOn == IsSoundEffectsOn)
                    {
                        return;
                    }

                    IsSoundEffectsOn = isOn;
                    break;
                case SoundType.Music:

                    if (isOn == IsMusicOn)
                    {
                        return;
                    }

                    IsMusicOn = isOn;
                    break;
            }

            // If it was turned off then stop all sounds that are currently playing
            if (!isOn)
            {
                Stop(type);
            }
        }
 

        /// <summary>
        /// Stops all sounds with the given id
        /// </summary>
        private void StopAllSounds(string id, List<PlayingSound> playingSounds)
        {
            for (int i = 0; i < playingSounds.Count; i++)
            {
                PlayingSound playingSound = playingSounds[i];

                if (id == playingSound.soundInfo.id)
                {
                    playingSound.audioSource.Stop();
                    Destroy(playingSound.audioSource.gameObject);
                    playingSounds.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Stops all sounds with the given type
        /// </summary>
        private void StopAllSounds(SoundType type, List<PlayingSound> playingSounds)
        {
            for (int i = 0; i < playingSounds.Count; i++)
            {
                PlayingSound playingSound = playingSounds[i];

                if (type == playingSound.soundInfo.type)
                {
                    playingSound.audioSource.Stop();
                    Destroy(playingSound.audioSource.gameObject);
                    playingSounds.RemoveAt(i);
                    i--;
                }
            }
        }

        private SoundInfo GetSoundInfo(string id)
        {
            for (int i = 0; i < soundInfos.Count; i++)
            {
                if (id == soundInfos[i].id)
                {
                    return soundInfos[i];
                }
            }

            return null;
        }

        private AudioSource CreateAudioSource(string id)
        {
            GameObject obj = new GameObject("sound_" + id);
            obj.transform.SetParent(transform);
            return obj.AddComponent<AudioSource>();
        }
    }
}