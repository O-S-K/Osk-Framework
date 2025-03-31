using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public class SoundSetup
    {
        public string id;
        public bool loop = false;
        public VolumeFade volume;
        public float playDelay = 0;
        public int priority = 128;
        public float pitch = 1;
        public Transform transform = null;
        public int minDistance = 1;
        public int maxDistance = 500;

        public SoundSetup(string id = "", bool loop = false, float playDelay = 0, int priority = 128, float pitch = 1,
            Transform transform = null, int minDistance = 1, int maxDistance = 500)
        {
            this.id = id;
            this.loop = loop;
            this.playDelay = playDelay;
            this.priority = priority;
            this.pitch = pitch;
            this.transform = transform;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
        }
    }

    public class VolumeFade
    {
        public float init = 0;
        public float target = 1;
        public float duration = 0; 

        public VolumeFade(float init = 0, float target = 1, float duration = 0, float fadeIn = 0, float fadeOut = 0)
        {
            this.init = init;
            this.target = target;
            this.duration = duration; 
        }
    }

    public partial class SoundManager
    {
        #region With Setup Data

        public void Play(SoundSetup soundSetup)
        {
            Play(soundSetup.id, soundSetup.volume, soundSetup.loop, soundSetup.playDelay, soundSetup.priority,
                soundSetup.pitch,
                soundSetup.transform, soundSetup.minDistance, soundSetup.maxDistance);
        }

        #endregion

        #region With ID

        public void Play(string id)
        {
            Play(id, new VolumeFade { target = 1 }, false, 0, 128, 1, null);
        }

        public void Play(string id, VolumeFade volume)
        {
            Play(id, volume, false, 0, 128, 1, null);
        }

        public void Play(string id, VolumeFade volume, bool loop)
        {
            Play(id, volume, loop, 0, 128, 1, null);
        }
  
        public void Play(string id, VolumeFade volume, bool loop, float playDelay)
        {
            Play(id, volume, loop, playDelay, 128, 1, null);
        }

        public void Play(string id, VolumeFade volume, bool loop, float playDelay, int priority)
        {
            Play(id, volume, loop, playDelay, priority, 1, null);
        }

        public void Play(string id, VolumeFade volume, bool loop, float playDelay, int priority, float pitch)
        {
            Play(id, volume, loop, playDelay, priority, pitch, null);
        }

        #endregion

        #region With ID and Transform

        public void Play3D(string id, VolumeFade volume, Transform transform, int minDistance = 1, int maxDistance = 500)
        {
            Play(id, volume, false, 0, 128, 1, transform, minDistance, maxDistance);
        }

        public void Play3D(string id, VolumeFade volume, Transform transform, int minDistance, int maxDistance, bool loop)
        {
            Play(id, volume, loop, 0, 128, 1, transform, minDistance, maxDistance);
        }

        public void Play3D(string id, VolumeFade volume, Transform transform, int minDistance, int maxDistance, bool loop, float playDelay)
        {
            Play(id, volume, loop, playDelay, 128, 1, transform, minDistance, maxDistance);
        }
        
        public void Play3D(string id, VolumeFade volume, Transform transform, int minDistance, int maxDistance, bool loop, float playDelay, float pitch)
        {
            Play(id, volume, loop, playDelay, 128, pitch, transform, minDistance, maxDistance);
        }

        #endregion

        #region With AudioClip

        public void Play(AudioClip audioClip)
        {
            PlayAudioClip(audioClip, new VolumeFade{target = 1}, false, 0, 128, 1, null);
        }

        public void Play(AudioClip audioClip, VolumeFade volume)
        {
            PlayAudioClip(audioClip, volume, false, 0, 128, 1, null);
        }

        public void Play(AudioClip audioClip, VolumeFade volume, bool loop)
        {
            PlayAudioClip(audioClip, volume, loop, 0, 128, 1, null);
        }

        public void Play(AudioClip audioClip, VolumeFade volume, bool loop, float playDelay)
        {
            PlayAudioClip(audioClip, volume, loop, playDelay, 128, 1, null);
        }

        public void Play(AudioClip audioClip, VolumeFade volume, bool loop, float playDelay, int priority)
        {
            PlayAudioClip(audioClip, volume, loop, playDelay, priority, 1, null);
        }

        public void Play(AudioClip audioClip, VolumeFade volume, bool loop, float playDelay, int priority, float pitch)
        {
            PlayAudioClip(audioClip, volume, loop, playDelay, priority, pitch, null);
        }

        public void Play(AudioClip audioClip, VolumeFade volume, bool loop, float playDelay, int priority, float pitch,
            Transform transform)
        {
            PlayAudioClip(audioClip, volume, loop, playDelay, priority, pitch, transform);
        }

        #endregion

        #region With AudioClip and Transform

        public void Play3D(AudioClip audioClip, VolumeFade volume, Transform transform, int minDistance = 1, int maxDistance = 500)
        {
            PlayAudioClip(audioClip, volume, false, 0, 128, 1, transform, minDistance, maxDistance);
        }

        public void Play3D(AudioClip audioClip, VolumeFade volume, Transform transform, int minDistance, int maxDistance, bool loop)
        {
            PlayAudioClip(audioClip, volume, loop, 0, 128, 1, transform, minDistance, maxDistance);
        }

        public void Play3D(AudioClip audioClip, VolumeFade volume, Transform transform, int minDistance, int maxDistance, bool loop,
            float playDelay)
        {
            PlayAudioClip(audioClip, volume, loop, playDelay, 128, 1, transform, minDistance, maxDistance);
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
            foreach (var playingSound in _listMusicInfos)
            {
                Destroy(playingSound.AudioSource.gameObject);
            }

            _listMusicInfos.Clear();
            Main.Pool.DestroyGroup(KeyGroupPool.AudioSound);
        }

        #endregion
    }
}