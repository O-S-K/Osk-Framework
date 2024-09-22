using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SoundData", menuName = "OSK/Sound/SoundData")]
public class SoundData : ScriptableObject
{
    [TableList, SerializeField] private List<SoundInfo> _listSoundInfos = new List<SoundInfo>();
    public List<SoundInfo> ListSoundInfos => _listSoundInfos;


#if UNITY_EDITOR
    [Button]
    private void SortToType()
    {
        if (_listSoundInfos == null || _listSoundInfos.Count == 0)
        {
            Debug.LogWarning("List Sound Infos is empty.");
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
            Debug.LogWarning("List Sound Infos is empty.");
            return;
        }
        for (int i = 0; i < _listSoundInfos.Count; i++)
        {
            _listSoundInfos[i].id = _listSoundInfos[i].audioClip.name.ToString();
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }

    private void OnValidate()
    {
        // check unique audio clip name
        
        if(_listSoundInfos == null || _listSoundInfos.Count == 0)
            return;
        
        var audioClipNames = new List<string>();
        foreach (var soundInfo in _listSoundInfos)
        {
            if (audioClipNames.Contains(soundInfo.audioClip.name))
            {
                Debug.LogError($"Audio Clip Name {soundInfo.audioClip.name} exists in the list. Please remove it.");
            }
            else
            {
                audioClipNames.Add(soundInfo.audioClip.name);
            }
        }
    }
#endif
}