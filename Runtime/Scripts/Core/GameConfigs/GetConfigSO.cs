#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

using UnityEditor;
namespace OSK
{
    public class GetConfigSO
    { 
        [Button]
        private void AddConfigs()
        {
            FindViewDataSOAssets();
            FindSoundDataSOAssets();
            FindImageDataSOAssets();
        }
        
        private void FindViewDataSOAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:ListViewSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No ViewData found in the project.");
                return;
            }

            List<ListViewSO> viewDatas = new List<ListViewSO>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ListViewSO v = AssetDatabase.LoadAssetAtPath<ListViewSO>(path);
                viewDatas.Add(v);
            }

            if (viewDatas.Count == 0)
            {
                Debug.LogError("No ViewData found in the project.");
                return;
            }
            else
            {
                foreach (ListViewSO v in viewDatas)
                {
                    Debug.Log("ViewData found: " + v.name);
                    var data = ScriptableObject.CreateInstance<ConfigInit>().data;
                    data.listViewS0 = v;
                }
            }
        }
        private void FindSoundDataSOAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:ListSoundSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No SoundData found in the project.");
                return;
            }

            List<ListSoundSO> soundDatas = new List<ListSoundSO>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ListSoundSO v = AssetDatabase.LoadAssetAtPath<ListSoundSO>(path);
                soundDatas.Add(v);
            }

            if (soundDatas.Count == 0)
            {
                Debug.LogError("No SoundData found in the project.");
                return;
            }
            else
            {
                foreach (ListSoundSO v in soundDatas)
                {
                    Debug.Log("SoundData found: " + v.name);
                    var data = ScriptableObject.CreateInstance<ConfigInit>().data;
                    data.listSoundSo = v;
                }
            }
        }
        private void FindImageDataSOAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:UIImageSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No ImageEffectData found in the project.");
                return;
            }

            List<UIParticleSO> imageEffectDatas = new List<UIParticleSO>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UIParticleSO v = AssetDatabase.LoadAssetAtPath<UIParticleSO>(path);
                imageEffectDatas.Add(v);
            }

            if (imageEffectDatas.Count == 0)
            {
                Debug.LogError("No ImageEffectData found in the project.");
                return;
            }
            else
            {
                foreach (UIParticleSO v in imageEffectDatas)
                {
                    Debug.Log("ImageEffectData found: " + v.name);
                    var data = ScriptableObject.CreateInstance<ConfigInit>().data;
                    data.uiParticleSO = v;
                }
            }
        }
    }
}
#endif
