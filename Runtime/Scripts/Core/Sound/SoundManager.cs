using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

        public bool IsEnableMusic = true;
        public bool IsEnableSoundSFX = true;
        
        private Tweener _tweener;
        private Transform _parentGroup;

 
        private AudioSource _soundObject;
        private Transform _cameraTransform;
        public Transform CameraTransform
        {
            get
            {
                if (_cameraTransform == null)
                {
                    _cameraTransform = Camera.main.transform;
                }

                return _cameraTransform;
            }
            set => _cameraTransform = value;
        }
 
        public override void OnInit()
        {
            if (Main.Configs.init == null || Main.Configs.init.data == null || Main.Configs.init.data.listSoundSo == null)
                return;

            _listSoundInfos = Main.Configs.init.data.listSoundSo.ListSoundInfos;
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
            {
                OSK.Logg.LogError("SoundInfos is empty");
                return;
            }

            _soundObject = new GameObject("AudioSource").AddComponent<AudioSource>();
            _soundObject.transform.parent = transform;
            _listMusicInfos = new List<PlayingSound>();

            maxCapacityMusic = Main.Configs.init.data.listSoundSo.maxCapacityMusic;
            maxCapacitySoundEffects = Main.Configs.init.data.listSoundSo.maxCapacitySFX;
        }

        private void Update() => CheckForStoppedMusic();
        
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

        public void PlayAudioClip(AudioClip audioClip, VolumeFade volume, bool loop, float playDelay, int priority,
            float pitch, Transform transform, int minDistance = 1, int maxDistance = 500)
        {
            if (loop && !IsEnableMusic || !IsEnableSoundSFX)
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

        public void Play(string id, VolumeFade volume, bool loop, float playDelay, int priority, float pitch, Transform transform,
            int minDistance = 1, int maxDistance = 500)
        {
            SoundData soundData = GetSoundInfo(id);

            if (soundData == null)
            {
                OSK.Logg.LogError("[Sound] There is no Sound Info with the given id: " + id);
                return ;
            }

            if (soundData.type == SoundType.MUSIC && !IsEnableMusic || soundData.type == SoundType.SFX && !IsEnableSoundSFX)
            {
                return ;
            }

            //  check capacity
            if (soundData.type == SoundType.MUSIC &&
                _listMusicInfos.Count(s => s.SoundData.type == SoundType.MUSIC) >= maxCapacityMusic)
            {
                RemoveOldestSound(SoundType.MUSIC);
            }
            else if (soundData.type == SoundType.SFX && _listMusicInfos.Count(s => s.SoundData.type == SoundType.SFX) >=
                     maxCapacitySoundEffects)
            {
                RemoveOldestSound(SoundType.SFX);
            }

            AudioSource audioSource = CreateAudioSource(id, soundData.audioClip, volume, loop, playDelay,
                priority, pitch, transform, minDistance, maxDistance);
            
            //Logg.Log($"1AudioSource: {audioSource.name}, activeSelf: {audioSource.gameObject.activeSelf}, activeInHierarchy: {audioSource.gameObject.activeInHierarchy}");

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

        private AudioSource CreateAudioSource(string id, AudioClip audioClip, VolumeFade volume, bool loop, float playDelay,
            int priority, float pitch, Transform transform, int minDistance, int maxDistance)
        {
            var audioSource = Main.Pool.Spawn(KeyGroupPool.AudioSound, _soundObject, _parentGroup != null ? _parentGroup : null);
            if (transform == null)
            {
                audioSource.spatialBlend = 0;
                audioSource.transform.position = CameraTransform.position;
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

            if (volume.duration > 0)
            {
                _tweener?.Kill();
                _tweener = DOVirtual.Float(volume.init, volume.target, volume.duration, value =>
                {
                    audioSource.volume = value;
                });
            }
            else
            {
                audioSource.volume = volume.target;
            }
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
        
        public void SetCameraTransform(Transform cameraTransform)
        { 
            CameraTransform = cameraTransform;
        }
        
        public void SetParentGroup(Transform parentGroup)
        {
            _parentGroup = parentGroup;
        }
        
        public void SetGroupDontDestroyOnLoad(bool isDontDestroyOnLoad)
        {
            if (isDontDestroyOnLoad)
            {
                _parentGroup.gameObject.GetOrAdd<DontDestroy>().DontDesGOOnLoad();
            }
            else
            {
                if (_parentGroup != null && _parentGroup.GetComponent<DontDestroy>() != null)
                {
                    Destroy(_parentGroup.GetComponent<DontDestroy>());
                }
            }
        }

        public void LogStatus()
        {
            Logg.Log("SoundManager Status");
            Logg.Log($"1.Main.Configs.init.data.listSoundSo: {Main.Configs.init.data.listSoundSo}");
            Logg.Log($"2.KeyGroupPool.AudioSound: {KeyGroupPool.AudioSound}");
            Logg.Log($"3.CameraTransform: {CameraTransform}");
            Logg.Log($"4.ParentGroup: {_parentGroup}");

            Logg.Log($"AudioListener: {AudioListener.pause}");
            Logg.Log($"5.IsEnableMusic: {IsEnableMusic}");
            Logg.Log($"6.IsEnableSoundSFX: {IsEnableSoundSFX}");

            Logg.Log($"7.MaxCapacityMusic: {maxCapacityMusic}");
            Logg.Log($"8.MaxCapacitySoundEffects: {maxCapacitySoundEffects}");
            
            Logg.Log($"9.ListSoundInfos: {_listSoundInfos.Count}");
            Logg.Log($"10.ListMusicInfos: {_listMusicInfos.Count}");
            
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
               Logg.Log($"_listMusicInfos[{i}]: {_listMusicInfos[i].SoundData.id}");
            }
            Logg.Log("End SoundManager Status");
        }
    }
}
