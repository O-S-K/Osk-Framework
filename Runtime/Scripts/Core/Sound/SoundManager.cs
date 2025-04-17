using System.Linq;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace OSK
{
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

 
        private Transform _cameraTransform; 
        public Transform CameraTransform
        {
            get
            {
                if (Camera.main != null) return _cameraTransform ??= Camera.main.transform;
                else
                {
                    OSK.Logg.LogError("Camera.main is null");
                    return null;
                }
            }
            set => _cameraTransform = value;
        }
        private AudioSource _soundObject;

        private bool pauseWhenInBackground = false;


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

#if UNITY_EDITOR
        private void OnApplicationPause(bool pause)
        {
            pauseWhenInBackground = pause;
        }
#endif

        
        private void Update() => CheckForStoppedMusic();
        
        private void CheckForStoppedMusic()
        {
            if (_listMusicInfos == null || _listMusicInfos.Count == 0)
                return;
            
#if UNITY_EDITOR
            if(pauseWhenInBackground)
                return;
#endif

            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                var playing = _listMusicInfos[i];
                if (playing.AudioSource != null && !playing.AudioSource.isPlaying && !playing.IsPaused)
                {
                    Main.Pool.Despawn(playing.AudioSource);
                    _listMusicInfos.RemoveAt(i--);
                }
            }
        }

        public AudioSource PlayAudioClip(AudioClip clip,SoundType soundType = SoundType.SFX, VolumeFade volume = null, float startTime = 0,
            bool loop = false, float delay = 0, int priority = 128, float pitch = 1,
            Transform target = null, int minDistance = 1, int maxDistance = 500)
        {
            if ((loop && !IsEnableMusic) || !IsEnableSoundSFX) return null;
            if (_listMusicInfos.Count >= maxCapacitySoundEffects) RemoveOldestSound(SoundType.SFX);

            void PlayNow()
            {
                var source = CreateAudioSource(clip.name, clip, startTime, volume, loop, delay,
                    priority, pitch, target, minDistance, maxDistance);
                _listMusicInfos.Add(new PlayingSound { AudioSource = source, SoundData = new SoundData {id = clip.name, audioClip = clip, type =  soundType} });
            }

            if (delay > 0) this.DoDelay(delay, PlayNow);
            else PlayNow();

            return _listMusicInfos.LastOrDefault()?.AudioSource;
        }

        public AudioSource Play(string id, VolumeFade volume = null, float startTime = 0,
            bool loop = false, float delay = 0, int priority = 128, float pitch = 1,
            Transform target = null, int minDistance = 1, int maxDistance = 500)
        {
            if(!IsEnableMusic && !IsEnableSoundSFX) return null;
            
            var data = GetSoundInfo(id);
            if (data == null)
            {
                Logg.LogError("[Sound] No Sound Info with id: " + id);
                return null;
            }

            if ((data.type == SoundType.MUSIC && !IsEnableMusic) ||
                (data.type == SoundType.SFX && !IsEnableSoundSFX)) return null;

            if (data.type == SoundType.MUSIC && _listMusicInfos.Count(s => s.SoundData.type == SoundType.MUSIC) >= maxCapacityMusic)
                RemoveOldestSound(SoundType.MUSIC);
            else if (data.type == SoundType.SFX && _listMusicInfos.Count(s => s.SoundData.type == SoundType.SFX) >= maxCapacitySoundEffects)
                RemoveOldestSound(SoundType.SFX);

            void PlayNow()
            {
                var source = CreateAudioSource(id, data.audioClip, startTime, volume, loop, delay,
                    priority, pitch, target, minDistance, maxDistance);
                _listMusicInfos.Add(new PlayingSound { AudioSource = source, SoundData = data });
            }

            if (delay > 0) this.DoDelay(delay, PlayNow);
            else PlayNow();

            return _listMusicInfos.LastOrDefault()?.AudioSource;
        }

        private void RemoveOldestSound(SoundType type)
        {
            var oldest = _listMusicInfos.FirstOrDefault(s => s.SoundData.type == type);
            if (oldest != null && oldest.AudioSource != null)
            {
                oldest.AudioSource.Stop();
                Destroy(oldest.AudioSource.gameObject);
                _listMusicInfos.Remove(oldest);
            }
        }
         
        private IEnumerator DespawnAudioSource(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            Main.Pool.Despawn(audioSource);
        }
 

        private AudioSource CreateAudioSource(string id, AudioClip clip, float startTime, VolumeFade volume, bool loop, float delay,
            int priority, float pitch, Transform target, int minDist, int maxDist)
        {
            var source = Main.Pool.Spawn(KeyGroupPool.AudioSound, _soundObject, _parentGroup);
            source.Stop();
            source.playOnAwake = false;
            source.name = id;
            source.clip = clip;
            source.loop = loop;
            source.priority = priority;
            source.pitch = pitch;
            source.minDistance = minDist;
            source.maxDistance = maxDist;
            
            if (startTime > 0) source.time = startTime;

            if (transform == null)
            {
                source.spatialBlend = 0; 
                source.transform.position = CameraTransform.position;
            }
            else
            {
                source.spatialBlend = 1;
                source.transform.position = transform.position;
            } 

            volume ??= new VolumeFade(1);
            if (volume.duration > 0)
            {
                _tweener?.Kill();
                _tweener = DOVirtual.Float(volume.init, volume.target, volume.duration, v => source.volume = v);
            }
            else source.volume = volume.target;
            
            /*source.outputAudioMixerGroup = data.mixerGroup;
            source.mute = data.mute;
            source.bypassEffects = data.bypassEffects;
            source.bypassListenerEffects = data.bypassListenerEffects;
            source.bypassReverbZones = data.bypassReverbZones;
            source.panStereo = data.panStereo;
            source.reverbZoneMix = data.reverbZoneMix;
            source.dopplerLevel = data.dopplerLevel;
            source.spread = data.spread;
            source.ignoreListenerVolume = data.ignoreListenerVolume;
            source.ignoreListenerPause = data.ignoreListenerPause;#1#*/
 
            source.Play();
            return source;
        }
        
        
        public SoundData GetSoundInfo(string id) => _listSoundInfos.FirstOrDefault(s => s.id == id);
        public SoundData GetSoundInfo(AudioClip audioClip) => _listSoundInfos.FirstOrDefault(s => s.audioClip == audioClip);

        
        public void SetCameraTransform(Transform cam) => CameraTransform = cam;
        public void SetParentGroup(Transform group, bool setDontDestroy)
        {
            _parentGroup = group;
            SetGroupDontDestroyOnLoad(setDontDestroy);
        }

        public void SetGroupDontDestroyOnLoad(bool enable)
        {
            if (_parentGroup == null)
            {
                OSK.Logg.LogError("ParentGroup Sound is null. Please set it before calling this method.");
                return;
            }

            var existing = _parentGroup.GetComponent<DontDestroy>();

            if (enable)
            {
                if (existing == null)
                    existing = _parentGroup.gameObject.AddComponent<DontDestroy>();
                existing.DontDesGOOnLoad();
            }
            else
            {
                if (existing != null)
                    UnityEngine.Object.Destroy(existing);
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
            Debug.Log($"11. RunInBackground: {Application.runInBackground}");
            Logg.Log("End SoundManager Status");
        }
    }
}
