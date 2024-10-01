using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class PlayingSound
    {
        public SoundInfo soundInfo = null;
        public AudioSource audioSource = null;
        public bool isPaused = false;
    }

    public class SoundManager : GameFrameworkComponent
    {
        [ShowInInspector, ReadOnly] private List<SoundInfo> soundInfos;
        [ShowInInspector, ReadOnly] private List<PlayingSound> MusicInfos;

        public SoundData soundData;
        private AudioSource soundObject;

        public bool isMusic { get; private set; }
        public bool isSoundEffects { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            soundObject = new GameObject().AddComponent<AudioSource>();
            soundObject.transform.parent = transform;
            MusicInfos = new List<PlayingSound>();

            isMusic = true;
            isSoundEffects = true;

            if (soundData != null)
            {
                soundInfos = soundData.ListSoundInfos;
            }
            else
            {
                Debug.LogWarning("SoundData is not assigned in SoundManager.");
            }
        }


        private void Update()
        {
            if (MusicInfos == null)
                return;

            for (int i = 0; i < MusicInfos.Count; i++)
            {
                AudioSource audioSource = MusicInfos[i].audioSource;

                // If the Audio Source is no longer playing then return it to the pool so it can be re-used
                if (!audioSource.isPlaying && !MusicInfos[i].isPaused)
                {
                    World.Pool.Release(audioSource);
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
            Play(id, loop, 0, 0);
        }

        /// <summary>
        ///  Plays the sound with the give id, if loop is set to true then the sound will only stop if the Stop method is called
        /// </summary>
        public void Play(string id, bool loop, float playDelay)
        {
            Play(id, loop, playDelay, 0);
        }

        /// <summary>
        ///  Plays the sound with the give id, if loop is set to true then the sound will only stop if the Stop method is called
        /// </summary>
        public void Play(string id, bool loop, int priority)
        {
            Play(id, loop, 0, priority);
        }

        public void Play(string id, float pitch)
        {
            Play(id, false, 0, 0, pitch);
        }

        /// <summary>
        ///  Plays the sound with the give id, if loop is set to true then the sound will only stop if the Stop method is called
        /// </summary>
        public void PlayAudioClip(AudioClip audioClip, float volume = 1, bool loop = false, float playDelay = 0,
            int priority = 0, float pitch = 1)
        {
            if ((loop && !isMusic) || (!isSoundEffects))
            {
                return;
            } 
            AudioSource audioSource = CreateAudioSource(audioClip.name);

            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.time = 0;
            audioSource.volume = volume;
            audioSource.priority = priority;
            audioSource.pitch = pitch;

            if (playDelay > 0)
            {
                audioSource.PlayDelayed(playDelay);
            }
            else
            {
                audioSource.Play();
            }

            MusicInfos.Add(new PlayingSound
                { audioSource = audioSource, soundInfo = new SoundInfo { audioClip = audioClip } });
        }


        /// <summary>
        ///  Releases the audio source after the given delay
        /// </summary>
        public void ReleaseDelayedAudioSource(AudioSource audioSource, float delay)
        {
            StartCoroutine(ReleaseAudioSource(audioSource, delay));
        }

        /// <summary>
        ///  Releases the audio source after the given delay
        /// </summary>
        private IEnumerator ReleaseAudioSource(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            World.Pool.Release(audioSource);
        }

        /// <summary>
        /// Plays the sound with the give id, if loop is set to true then the sound will only stop if the Stop method is called
        /// </summary>
        public void Play(string id, bool loop, float playDelay, int priority, float pitch = 1)
        {
            SoundInfo soundInfo = GetSoundInfo(id);

            if (soundInfo == null)
            {
                Debug.LogError("[SoundManager] There is no Sound Info with the given id: " + id);

                return;
            }

            if ((soundInfo.type == SoundType.Music && !isMusic) ||
                (soundInfo.type == SoundType.SoundEffect && !isSoundEffects))
            {
                return;
            }

            AudioSource audioSource = CreateAudioSource(id);

            audioSource.clip = soundInfo.audioClip;
            audioSource.loop = loop;
            audioSource.time = 0;
            audioSource.volume = soundInfo.clipVolume;
            audioSource.priority = priority;
            audioSource.pitch = pitch;

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

                    if (isOn == isSoundEffects)
                    {
                        return;
                    }

                    isSoundEffects = isOn;
                    break;
                case SoundType.Music:

                    if (isOn == isMusic)
                    {
                        return;
                    }

                    isMusic = isOn;
                    break;
            }

            // If it was turned off then stop all sounds that are currently playing
            if (!isOn)
            {
                Stop(type);
            }
        }


        /// <summary>
        ///  Sets the SoundType on/off
        /// </summary>
        public void SetSoundAllOnOff(bool isOn)
        {
            isMusic = isOn;
            isSoundEffects = isOn;

            if (!isOn)
            {
                PauseAll();
            }
            else
            {
                ResumeAll();
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
        /// Pauses all sounds
        /// </summary>
        public void PauseAll()
        {
            foreach (var playingSound in MusicInfos)
            {
                playingSound.audioSource.Pause();
                playingSound.isPaused = true;
            }
        }

        /// <summary>
        ///  Resumes all paused sounds
        /// </summary>
        public void ResumeAll()
        {
            foreach (var playingSound in MusicInfos)
            {
                playingSound.audioSource.UnPause();
                playingSound.isPaused = false;
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
            var audio = World.Pool.Create("AudioSource", soundObject);
            audio.transform.position = Camera.main.transform.position;
            audio.name = id;
            return audio;
        }

        public void DestroyAll()
        {
            foreach (var playingSound in MusicInfos)
            {
                Destroy(playingSound.audioSource.gameObject);
            }

            MusicInfos.Clear();
            World.Pool.DestroyGroup("AudioSource");
        }
    }
}