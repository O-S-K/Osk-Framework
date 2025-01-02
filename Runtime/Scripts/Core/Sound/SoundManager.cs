using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
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

    public partial class SoundManager : GameFrameworkComponent
    {
        [ShowInInspector] private List<SoundData> _listSoundInfos = new List<SoundData>();
        [ShowInInspector] private List<PlayingSound> _listMusicInfos = new List<PlayingSound>();

        public List<SoundData> GetListSoundInfos => _listSoundInfos;
        public List<PlayingSound> GetListMusicInfos => _listMusicInfos;

        [SerializeField] private int maxCapacityMusic = 5;
        [SerializeField] private int maxCapacitySoundEffects = 10;

        public bool IsMusic
        {
            get => PlayerPrefs.GetInt("IsMusic", 1) == 1;
            set => PlayerPrefs.SetInt("IsMusic", value ? 1 : 0);
        }

        public bool IsSoundSFX
        {
            get => PlayerPrefs.GetInt("IsSoundSFX", 1) == 1;
            set => PlayerPrefs.SetInt("IsSoundSFX", value ? 1 : 0);
        }


        private AudioSource _soundObject;

        private Transform cameraTransform
        {
            get
            {
                if (Camera.main == null)
                {
                    OSK.Logg.LogError("Camera.main is null");
                    return null;
                }

                return Camera.main.transform;
            }
        }
 
        public override void OnInit()
        {
            if (Main.Configs.init == null || Main.Configs.init.data == null || Main.Configs.init.data.soundDataSO == null)
                return;

            _listSoundInfos = Main.Configs.init.data.soundDataSO.ListSoundInfos;
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
            {
                OSK.Logg.LogError("SoundInfos is empty");
                return;
            }

            _soundObject = new GameObject("AudioSource").AddComponent<AudioSource>();
            _soundObject.transform.parent = transform;
            _listMusicInfos = new List<PlayingSound>();

            maxCapacityMusic = Main.Configs.init.data.soundDataSO.maxCapacityMusic;
            maxCapacitySoundEffects = Main.Configs.init.data.soundDataSO.maxCapacitySFX;
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
                AudioSource audioSource = _listMusicInfos[i].AudioSource;

                // If the Audio Source is no longer playing then return it to the pool so it can be re-used
                if (audioSource != null && !audioSource.isPlaying && !_listMusicInfos[i].IsPaused)
                {
                    Main.Pool.Despawn(audioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        private IEnumerator DespawnAudioSource(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            Main.Pool.Despawn(audioSource);
        }

        public void PlayAudioClip(AudioClip audioClip, float volume, bool loop, float playDelay, int priority,
            float pitch, Transform transform, int minDistance = 1, int maxDistance = 500)
        {
            if (loop && !IsMusic || !IsSoundSFX)
            {
                return;
            }

            //  check capacity
            if (_listMusicInfos.Count >= maxCapacitySoundEffects)
            {
                RemoveOldestSound(SoundType.SFX);
            }

            AudioSource audioSource = CreateAudioSource(audioClip.name, audioClip, volume, loop, playDelay, priority,
                pitch, transform, minDistance, maxDistance);

            _listMusicInfos.Add(new PlayingSound
            {
                AudioSource = audioSource,
                SoundData = new SoundData { audioClip = audioClip }
            });
        }

        public void Play(string id, bool loop, float playDelay, int priority, float pitch, Transform transform,
            int minDistance = 1, int maxDistance = 500)
        {
            SoundData soundData = GetSoundInfo(id);

            if (soundData == null)
            {
                OSK.Logg.LogError("[Sound] There is no Sound Info with the given id: " + id);
                return;
            }

            if (soundData.type == SoundType.Music && !IsMusic || soundData.type == SoundType.SFX && !IsSoundSFX)
            {
                return;
            }

            //  check capacity
            if (soundData.type == SoundType.Music &&
                _listMusicInfos.Count(s => s.SoundData.type == SoundType.Music) >= maxCapacityMusic)
            {
                RemoveOldestSound(SoundType.Music);
            }
            else if (soundData.type == SoundType.SFX && _listMusicInfos.Count(s => s.SoundData.type == SoundType.SFX) >=
                     maxCapacitySoundEffects)
            {
                RemoveOldestSound(SoundType.SFX);
            }

            AudioSource audioSource = CreateAudioSource(id, soundData.audioClip, soundData.volume, loop, playDelay,
                priority, pitch, transform, minDistance, maxDistance);

            PlayingSound playingSound = new PlayingSound();
            playingSound.SoundData = soundData;
            playingSound.AudioSource = audioSource;
            _listMusicInfos.Add(playingSound);
        }

        private void RemoveOldestSound(SoundType soundType)
        {
            PlayingSound oldestSound = _listMusicInfos.FirstOrDefault(s => s.SoundData.type == soundType);
            if (oldestSound != null && oldestSound.IsPlaying)
            {
                oldestSound.AudioSource.Stop();
                Destroy(oldestSound.AudioSource.gameObject);
                _listMusicInfos.Remove(oldestSound);
            }
        }

        private SoundData GetSoundInfo(string id)
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

        private AudioSource CreateAudioSource(string id, AudioClip audioClip, float volume, bool loop, float playDelay,
            int priority, float pitch, Transform transform, int minDistance, int maxDistance)
        {
            var audioSource = Main.Pool.Spawn(KeyGroupPool.AudioSound, _soundObject, null);
            if (transform == null)
            {
                audioSource.spatialBlend = 0;
                audioSource.transform.position = cameraTransform.position;
            }
            else
            {
                audioSource.spatialBlend = 1;
                audioSource.transform.position = transform.position;
            }

            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;

            audioSource.name = id;
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.time = 0;
            audioSource.volume = volume;
            audioSource.priority = priority;
            audioSource.pitch = pitch;

            /*audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.mute = data.mute;
            audioSource.bypassEffects = data.bypassEffects;
            audioSource.bypassListenerEffects = data.bypassListenerEffects;
            audioSource.bypassReverbZones = data.bypassReverbZones;
            audioSource.panStereo = data.panStereo;
            audioSource.reverbZoneMix = data.reverbZoneMix;
            audioSource.dopplerLevel = data.dopplerLevel;
            audioSource.spread = data.spread;
            audioSource.ignoreListenerVolume = data.ignoreListenerVolume;
            audioSource.ignoreListenerPause = data.ignoreListenerPause;*/

            if (playDelay > 0)
            {
                audioSource.PlayDelayed(playDelay);
            }
            else
            {
                audioSource.Play();
            }

            return audioSource;
        }
    }
}