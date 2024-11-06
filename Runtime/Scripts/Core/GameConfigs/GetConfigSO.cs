#if UNITY_EDITOR
using CustomInspector;
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
                    var data = ScriptableObject.CreateInstance<ConfigInitDefault>().data;
                    data.listViewS0 = v;
                }
            }
        }
        private void FindSoundDataSOAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:SoundDataSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No SoundData found in the project.");
                return;
            }

            List<SoundDataSO> soundDatas = new List<SoundDataSO>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SoundDataSO v = AssetDatabase.LoadAssetAtPath<SoundDataSO>(path);
                soundDatas.Add(v);
            }

            if (soundDatas.Count == 0)
            {
                Debug.LogError("No SoundData found in the project.");
                return;
            }
            else
            {
                foreach (SoundDataSO v in soundDatas)
                {
                    Debug.Log("SoundData found: " + v.name);
                    var data = ScriptableObject.CreateInstance<ConfigInitDefault>().data;
                    data.soundDataSO = v;
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

            List<UIImageSO> imageEffectDatas = new List<UIImageSO>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UIImageSO v = AssetDatabase.LoadAssetAtPath<UIImageSO>(path);
                imageEffectDatas.Add(v);
            }

            if (imageEffectDatas.Count == 0)
            {
                Debug.LogError("No ImageEffectData found in the project.");
                return;
            }
            else
            {
                foreach (UIImageSO v in imageEffectDatas)
                {
                    Debug.Log("ImageEffectData found: " + v.name);
                    var data = ScriptableObject.CreateInstance<ConfigInitDefault>().data;
                    data.uiImageSO = v;
                }
            }
        }
    }
}
#endif
