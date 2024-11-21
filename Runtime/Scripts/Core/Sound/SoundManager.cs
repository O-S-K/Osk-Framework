using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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
        private const string keyPoolSound = "AudioSource";

        [SerializeField, ReadOnly] private List<SoundInfo> _listSoundInfos = new List<SoundInfo>();
        private List<PlayingSound> _listMusicInfos = new List<PlayingSound>();
        public List<SoundInfo> GetListSoundInfos => _listSoundInfos;
        public List<PlayingSound> GetListMusicInfos => _listMusicInfos;
        private AudioSource _soundObject;

        public bool isMusic { get; private set; }
        public bool isSoundEffects { get; private set; }
          
        public override void OnInit()
        {
            if(Main.Configs.Game.data.isUseSound == false)
                return;
            
            _soundObject = new GameObject("AudioSource").AddComponent<AudioSource>();
            _soundObject.transform.parent = transform;
            _listMusicInfos = new List<PlayingSound>();

            isMusic = true;
            isSoundEffects = true; 
            
            _listSoundInfos = Main.Configs.Game.data.soundDataSO.ListSoundInfos;
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
            {
                OSK.Logg.LogError("SoundInfos is empty");
                return;
            }
        }

        private void Update()
        {
            CheckForStoppedMusic();
        }

        private void CheckForStoppedMusic()
        {
            if (_listMusicInfos == null || _listMusicInfos.Count == 0)
                return;

            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                AudioSource audioSource = _listMusicInfos[i].audioSource;

                // If the Audio Source is no longer playing then return it to the pool so it can be re-used
                if (audioSource != null && !audioSource.isPlaying && !_listMusicInfos[i].isPaused)
                {
                    Main.Pool.Despawn(audioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Play(string id)
        {
            Play(id, false, 0, 0);
        }


        public void Play(string id, bool loop)
        {
            Play(id, loop, 0, 0);
        }

        public void Play(string id, bool loop, float playDelay)
        {
            Play(id, loop, playDelay, 0);
        }

        public void Play(string id, bool loop, int priority)
        {
            Play(id, loop, 0, priority);
        }

        public void Play(string id, float pitch)
        {
            Play(id, false, 0, 0, pitch);
        }

        public void SetVolume(float volume)
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
                return;
            for (int i = 0; i < _listSoundInfos.Count; i++)
            {
                _listSoundInfos[i].clipVolume = volume;
                return;
            }
        }

        public void SetVolume(SoundType type, float volume)
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
                return;

            foreach (var s in _listSoundInfos)
            {
                if (s.type == type)
                {
                    s.clipVolume = volume;
                    return;
                }
            }
        }

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

            _listMusicInfos.Add(new PlayingSound
                { audioSource = audioSource, soundInfo = new SoundInfo { audioClip = audioClip } });
        }

        public void ReleaseDelayedAudioSource(AudioSource audioSource, float delay)
        {
            StartCoroutine(ReleaseAudioSource(audioSource, delay));
        }

        private IEnumerator ReleaseAudioSource(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            Main.Pool.Despawn(audioSource);
        }

        public void Play(string id, bool loop, float playDelay, int priority, float pitch = 1)
        {
            SoundInfo soundInfo = GetSoundInfo(id);

            if (soundInfo == null)
            {
                OSK.Logg.LogError("[Sound] There is no Sound Info with the given id: " + id);

                return;
            }

            if ((soundInfo.type == SoundType.Music && !isMusic) ||
                (soundInfo.type == SoundType.SFX && !isSoundEffects))
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

            _listMusicInfos.Add(playingSound);
        }

        public void Stop(string id)
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                if (_listMusicInfos[i].soundInfo.id == id)
                {
                    _listMusicInfos[i].audioSource.Stop();
                    Main.Pool.Despawn(_listMusicInfos[i].audioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Stop(SoundType type)
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                if (_listMusicInfos[i].soundInfo.type == type)
                {
                    _listMusicInfos[i].audioSource.Stop();
                    Main.Pool.Despawn(_listMusicInfos[i].audioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        public void SetStatusSoundType(SoundType type, bool isOn)
        {
            switch (type)
            {
                case SoundType.Music:
                    isMusic = isOn;
                    if (isMusic)
                        Resume(SoundType.Music);
                    else
                        Pause(SoundType.Music);
                    break;
                case SoundType.SFX:
                    isSoundEffects = isOn;
                    if (isSoundEffects)
                        Resume(SoundType.SFX);
                    else
                        Pause(SoundType.SFX);
                    break;
            }
        }

        public void SetStatusAllSound(bool isOn)
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

        public void PauseAll()
        {
            foreach (var playingSound in _listMusicInfos)
            {
                playingSound.audioSource.Pause();
                playingSound.isPaused = true;
            }
        }

        public void Pause(SoundType type)
        {
            foreach (var playingSound in _listMusicInfos)
            {
                if (playingSound.soundInfo.type == type)
                {
                    playingSound.audioSource.Pause();
                    playingSound.isPaused = true;
                }
            }
        }

        public void ResumeAll()
        {
            foreach (var playingSound in _listMusicInfos)
            {
                playingSound.audioSource.UnPause();
                playingSound.isPaused = false;
            }
        }

        public void Resume(SoundType type)
        {
            foreach (var playingSound in _listMusicInfos)
            {
                if (playingSound.soundInfo.type == type)
                {
                    playingSound.audioSource.UnPause();
                    playingSound.isPaused = false;
                }
            }
        }

        private SoundInfo GetSoundInfo(string id)
        {
            for (int i = 0; i < _listSoundInfos.Count; i++)
            {
                if (id == _listSoundInfos[i].id)
                {
                    return _listSoundInfos[i];
                }
            }

            return null;
        }

        private AudioSource CreateAudioSource(string id)
        {
            var audioSource = Main.Pool.Spawn(keyPoolSound, _soundObject);
            if (Camera.main != null)
                audioSource.transform.position = Camera.main.transform.position;
            audioSource.name = id;
            return audioSource;
        }

        public void DestroyAll()
        {
            foreach (var playingSound in _listMusicInfos)
            {
                Destroy(playingSound.audioSource.gameObject);
            }

            _listMusicInfos.Clear();
            Main.Pool.DestroyGroup(keyPoolSound);
        }
    }
}