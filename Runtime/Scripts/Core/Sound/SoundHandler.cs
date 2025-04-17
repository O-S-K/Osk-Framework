using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace OSK
{
    [System.Serializable]
    [InlineProperty]
    public class SoundSetup
    {
        [LabelWidth(100)]
        public string id;

        [LabelWidth(100)] public AudioClip audioClip = null;

        [EnumToggleButtons] [LabelWidth(100)] public SoundType type = SoundType.SFX;

        [LabelWidth(100)] public float startTime = 0;
        [LabelWidth(100)] public bool loop = false;

        [FoldoutGroup("Advanced", expanded: false)]
        [LabelText("VolumeFade")]
        [LabelWidth(100)]
        public VolumeFade volumeFade = VolumeFade.Default;

        [FoldoutGroup("Advanced")]
        [PropertyTooltip("Higher = more important")]
        [PropertyRange(0, 256)]
        [LabelWidth(100)]
        public int priority = 128;

        [FoldoutGroup("Advanced")] [Range(0.1f, 3f)] [LabelWidth(100)]
        public float pitch = 1;

        [FoldoutGroup("Advanced")] [PropertyRange(0, 10)] [LabelWidth(100)]
        public float playDelay = 0;

        [FoldoutGroup("3D Settings")] [LabelWidth(100)]
        public Transform transform = null;

        [FoldoutGroup("3D Settings")] [LabelWidth(100)] [MinValue(0)]
        public int minDistance = 1;

        [FoldoutGroup("3D Settings")] [LabelWidth(100)] [MinValue(0)]
        public int maxDistance = 500;

        public SoundSetup(string id = "", AudioClip audioClip = null, SoundType type = SoundType.SFX,
            float startTime = 0, bool loop = false, VolumeFade volume = null, float playDelay = 0, int priority = 128,
            float pitch = 1, Transform transform = null, int minDistance = 1, int maxDistance = 500)
        {
            this.id = id;
            this.audioClip = audioClip;
            this.type = type;
            this.startTime = startTime;
            this.loop = loop;
            this.volumeFade = volume ?? new VolumeFade();
            this.playDelay = playDelay;
            this.priority = priority;
            this.pitch = pitch;
            this.transform = transform;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
        }

        public SoundSetup()
        {
            id = "";
            audioClip = null;
            type = SoundType.SFX;
            startTime = 0;
            loop = false;
            volumeFade = VolumeFade.Default;
            playDelay = 0;
            priority = 128;
            pitch = 1;
            transform = null;
            minDistance = 1;
            maxDistance = 500;
        }
    }

    [System.Serializable]
    public class VolumeFade
    {
        [LabelText("Init Volume")]
        [HorizontalGroup("Volume")]
        [LabelWidth(100)] [Min(0)] public float init = 0;
        [HorizontalGroup("Volume")]
        [LabelWidth(100)] [Min(0)] public float target = 1;
        [HorizontalGroup("Volume")]
        [LabelWidth(100)] [Min(0)] public float duration = 0;

        public VolumeFade(float init = 0, float target = 1, float duration = 0)
        {
            this.init = init;
            this.target = target;
            this.duration = duration;
        }

        public static VolumeFade Default => new VolumeFade(0, 1, 0);
    }

    public partial class SoundManager
    {
        #region With SoundSetup

        /// <summary>
        ///  Play a sound with Id in list Sound SO using SoundSetup
        /// </summary>
        /// <param name="soundSetup"></param>
        /// <returns></returns>
        public AudioSource PlayID(SoundSetup soundSetup)
        {
            return Play(soundSetup.id, soundSetup.volumeFade, soundSetup.startTime, soundSetup.loop,
                soundSetup.playDelay,
                soundSetup.priority,
                soundSetup.pitch,
                soundSetup.transform, soundSetup.minDistance, soundSetup.maxDistance);
        }

        /// <summary>
        ///  Play a sound with ClipAudio using SoundSetup
        /// </summary>
        /// <param name="soundSetup"></param>
        /// <returns></returns>
        public AudioSource PlayClip(SoundSetup soundSetup)
        {
            return PlayAudioClip(soundSetup.audioClip, soundSetup.type, soundSetup.volumeFade, soundSetup.startTime,
                soundSetup.loop, soundSetup.playDelay,
                soundSetup.priority,
                soundSetup.pitch,
                soundSetup.transform, soundSetup.minDistance, soundSetup.maxDistance);
        }

        #endregion

        #region Stop and Pause

        public void StopAll()
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                _listMusicInfos[i].AudioSource.Stop();
                Main.Pool.Despawn(_listMusicInfos[i].AudioSource);
                _listMusicInfos.RemoveAt(i);
                i--;
            }
        }

        public void Stop(string id)
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                if (_listMusicInfos[i].SoundData.id == id)
                {
                    _listMusicInfos[i].AudioSource.Stop();
                    Main.Pool.Despawn(_listMusicInfos[i].AudioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Stop(AudioClip clip)
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                if (_listMusicInfos[i].AudioSource.clip == clip)
                {
                    _listMusicInfos[i].AudioSource.Stop();
                    Main.Pool.Despawn(_listMusicInfos[i].AudioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }


        public void Stop(SoundType type)
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                if (_listMusicInfos[i].SoundData.type == type)
                {
                    _listMusicInfos[i].AudioSource.Stop();
                    Main.Pool.Despawn(_listMusicInfos[i].AudioSource);
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }


        public void PauseAll()
        {
            foreach (var playingSound in _listMusicInfos)
            {
                playingSound.AudioSource.Pause();
                playingSound.IsPaused = true;
            }
        }

        public void Pause(SoundType type)
        {
            foreach (var playingSound in _listMusicInfos)
            {
                if (playingSound.SoundData.type == type)
                {
                    playingSound.IsPaused = true;
                    playingSound.AudioSource.Pause();
                }
            }
        }

        public void ResumeAll()
        {
            foreach (var playingSound in _listMusicInfos)
            {
                playingSound.IsPaused = false;
                playingSound.AudioSource.UnPause();
            }
        }

        public void Resume(SoundType type)
        {
            foreach (var playingSound in _listMusicInfos)
            {
                if (playingSound.SoundData.type == type)
                {
                    playingSound.AudioSource.UnPause();
                    playingSound.IsPaused = false;
                }
            }
        }

        #endregion

        #region Status

        public void SetMixerGroup(AudioMixerGroup mixerGroup)
        {
            if (_listMusicInfos == null || _listMusicInfos.Count == 0) return;
            foreach (var playing in _listMusicInfos.Where(playing => playing.AudioSource != null))
            {
                playing.AudioSource.outputAudioMixerGroup = mixerGroup;
            }
        }

        public void SetStatusSoundType(SoundType type, bool isOn)
        {
            switch (type)
            {
                case SoundType.MUSIC:
                    IsEnableMusic = isOn;
                    if (IsEnableMusic)
                        Resume(SoundType.MUSIC);
                    else
                        Pause(SoundType.MUSIC);
                    break;
                case SoundType.SFX:
                    IsEnableSoundSFX = isOn;
                    if (IsEnableSoundSFX)
                        Resume(SoundType.SFX);
                    else
                        Pause(SoundType.SFX);
                    break;
            }
        }


        public void SetStatusAllSound(bool isOn)
        {
            IsEnableMusic = isOn;
            IsEnableSoundSFX = isOn;

            if (!isOn)
            {
                PauseAll();
            }
            else
            {
                ResumeAll();
            }
        }

        public void SetAllVolume(float volume)
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
                return;
            for (int i = 0; i < _listSoundInfos.Count; i++)
            {
                _listSoundInfos[i].volume = volume;
                return;
            }
        }

        public void SetAllVolume(SoundType type, float volume)
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
                return;

            foreach (var s in _listSoundInfos)
            {
                if (s.type == type)
                {
                    s.volume = volume;
                    return;
                }
            }
        }

        public float VolumeFade(float volumeStart, float volumeEnd, float duration)
        {
            float volume = 0;
            DOVirtual.Float(volumeStart, volumeEnd, duration, value => { volume = value; });
            return volume;
        }

        public AudioClip GetAudioClipInSO(string id)
        {
            foreach (var t in _listSoundInfos)
            {
                if (t.id == id) return t.audioClip;
            }

            return null;
        }

        public AudioClip GetAudioClipInSO(AudioClip audioClip)
        {
            foreach (var t in _listSoundInfos)
            {
                if (t.audioClip == audioClip) return t.audioClip;
            }

            return null;
        }

        public AudioClip GetAudioClipOnScene(string id)
        {
            foreach (var t in _listMusicInfos)
            {
                if (t.SoundData.id == id) return t.AudioSource.clip;
            }

            return null;
        }

        #endregion

        #region Dispose

        public void Despawn(AudioSource audioSource, float delay = 0)
        {
            DespawnAudioSource(audioSource, delay).Run();
        }

        public void DespawnMusicID(string id, float delay = 0)
        {
            for (int i = 0; i < _listMusicInfos.Count; i++)
            {
                if (_listMusicInfos[i].SoundData.id == id)
                {
                    DespawnAudioSource(_listMusicInfos[i].AudioSource, delay).Run();
                    _listMusicInfos.RemoveAt(i);
                    i--;
                }
            }
        }

        public void DestroyAll()
        {
            for (int i = _listMusicInfos.Count - 1; i >= 0; i--)
            {
                Destroy(_listMusicInfos[i].AudioSource);
            }

            _listMusicInfos.Clear();
            Main.Pool.DestroyAllInGroup(KeyGroupPool.AudioSound);
        }

        #endregion
    }
}