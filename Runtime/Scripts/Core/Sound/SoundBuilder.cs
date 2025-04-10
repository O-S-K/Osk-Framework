using UnityEngine;

namespace OSK
{
    public class SoundBuilder
    {
        private string _id;
        private float _startTime = 0;
        private bool _loop = false;
        private float _playDelay = 0;
        
        private VolumeFade _volume;
        private int _priority = 128;
        private float _pitch = 1;
        private int _minDistance = 1;
        private int _maxDistance = 500;

        private Transform _transform = null;
        private AudioClip _audioClip = null;

        public SoundBuilder()
        {
        }

        public SoundBuilder SetId(string id)
        {
            _id = id;
            return this;
        }
        
        public SoundBuilder SetStartTime(float startTime)
        {
            _startTime = startTime;
            return this;
        }

        public SoundBuilder SetLoop(bool loop)
        {
            _loop = loop;
            return this;
        }

        public SoundBuilder SetPlayDelay(float playDelay)
        {
            _playDelay = playDelay;
            return this;
        }

        public SoundBuilder SetPriority(int priority)
        {
            _priority = priority;
            return this;
        }

        public SoundBuilder SetPitch(float pitch)
        {
            _pitch = pitch;
            return this;
        }

        public SoundBuilder Set3D(Transform transform, int minDistance = 1, int maxDistance = 500)
        {
            _transform = transform;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            return this;
        }

        public SoundBuilder SetVolume(VolumeFade volume)
        {
            _volume = volume;
            return this;
        }


        public SoundBuilder SetAudioClip(AudioClip audioClip)
        {
            _audioClip = audioClip;
            return this;
        }

        public SoundBuilder PlayWithID()
        {
#if UNITY_EDITOR
            Validate();
#endif
            Main.Sound.Play(_id,_volume, _startTime, _loop, _playDelay, _priority, _pitch, _transform, _minDistance, _maxDistance);
            return this;
        }

        public SoundBuilder PlayWithClip()
        {
#if UNITY_EDITOR
            Validate();
#endif
            Main.Sound.PlayAudioClip(_audioClip, _volume, _startTime, _loop, _playDelay, _priority, _pitch, _transform, _minDistance, _maxDistance);
            return this;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(_id) && _audioClip == null)
            {
                Logg.LogError("Either ID or AudioClip must be set.");
            }

            if (_transform == null && (_minDistance != 1 || _maxDistance != 500))
            {
                if (!string.IsNullOrEmpty(_id))
                {
                    Logg.LogError($"Audio {_id} Transform must be set for 3D sound.");
                }
                else if (_audioClip != null)
                {
                    Logg.LogError($"Audio {_audioClip.name} Transform must be set for 3D sound.");
                }
                else
                {
                    Logg.LogError("Transform must be set for 3D sound.");
                }
            }
        }
    }
}