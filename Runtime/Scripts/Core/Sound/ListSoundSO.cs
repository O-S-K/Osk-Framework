using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    [CreateAssetMenu(fileName = "ListSoundSO", menuName = "OSK/Sound/ListSoundSO")]
    public class ListSoundSO : ScriptableObject
    {
        [HideInInspector]
        public string filePathSoundID = "Assets";
        
        [Space]
        [Title("Max Capacity")]
        public int maxCapacityMusic = 5;
        public int maxCapacitySFX = 25;
        
        [Space]
        [Title("List Sound Infos")]
        [TableList,SerializeField] 
        private List<SoundData> _listSoundInfos = new List<SoundData>();
        public List<SoundData> ListSoundInfos => _listSoundInfos;


#if UNITY_EDITOR
        [Button]
        private void SortToType()
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
            {
                OSK.Logg.LogWarning("List Sound Infos is empty.");
                return;
            }

            _listSoundInfos.Sort((a, b) => a.type.CompareTo(b.type));
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [Button]
        private void SetIDForNameClip()
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
            {
                OSK.Logg.LogWarning("List Sound Infos is empty.");
                return;
            }

            for (int i = 0; i < _listSoundInfos.Count; i++)
            {
                var name = _listSoundInfos[i].audioClip.name;
                _listSoundInfos[i].id = name;
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        public bool IsSoundExist(string soundName)
        {
            if (_listSoundInfos == null || _listSoundInfos.Count == 0)
                return false;
            
            foreach (var soundInfo in _listSoundInfos)
            {
                if (soundInfo.audioClip.name == soundName)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnValidate()
        {
            // check unique audio clip name in the list
            var audioClipNames = new List<string>();
            foreach (var soundInfo in _listSoundInfos)
            {
                if(soundInfo.audioClip == null || string.IsNullOrEmpty(soundInfo.audioClip.name))
                    continue;
                if (audioClipNames.Contains(soundInfo.audioClip.name))
                {
                    OSK.Logg.LogError(
                        $"Audio Clip Name {soundInfo.audioClip.name} exists in the list. Please remove it.");
                }
                else
                { 
                    audioClipNames.Add(soundInfo.audioClip.name);
                }
            }
        }
#endif
    }
}