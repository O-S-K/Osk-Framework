using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    [CreateAssetMenu(fileName = "ConfigInitDefault", menuName = "OSK/Configs/ConfigInitDefault", order = 0)]
    public class ConfigInitDefault : ScriptableObject
    {
        public int targetFrameRate = 60;
        public int vSyncCount = 0;
        
        public string packageName = "";
        public string appstoreID = ""; 

        public DataConfigs data;
        public SettingConfigs setting;
        public PathConfigs path;

#if UNITY_EDITOR
        [CustomInspector.Button]
        private void AddConfigs()
        {
            packageName = Application.identifier;
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSyncCount;
                
            FindViewDataSOAssets();
            FindSoundDataSOAssets();
            FindImageDataSOAssets();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        private void FindViewDataSOAssets()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ListViewSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No ViewData found in the project.");
                return;
            }

            List<ListViewSO> viewDatas = new List<ListViewSO>();
            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                ListViewSO v = UnityEditor.AssetDatabase.LoadAssetAtPath<ListViewSO>(path);
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
                    data.listViewS0 = v;
                }
            }
        }

        private void FindSoundDataSOAssets()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:SoundDataSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No SoundData found in the project.");
                return;
            }

            List<SoundDataSO> soundDatas = new List<SoundDataSO>();
            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                SoundDataSO v = UnityEditor.AssetDatabase.LoadAssetAtPath<SoundDataSO>(path);
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
                    data.soundDataSO = v;
                }
            }
        }

        private void FindImageDataSOAssets()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:UIImageSO");
            if (guids.Length == 0)
            {
                Debug.LogError("No ImageEffectData found in the project.");
                return;
            }

            List<UIImageSO> imageEffectDatas = new List<UIImageSO>();
            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                UIImageSO v = UnityEditor.AssetDatabase.LoadAssetAtPath<UIImageSO>(path);
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
                    data.uiImageSO = v;
                }
            }
        }
#endif
    }


    [System.Serializable]
    public class SettingConfigs
    {
        public bool isMusicOnDefault = true;
        public bool isSoundOnDefault = true;
        public bool isVibrationOnDefault = true;
        public bool isCheckInternetDefault = true;
        public SystemLanguage languageDefault = SystemLanguage.English;
    }

    [System.Serializable]
    public class DataConfigs
    {
        public ListViewSO listViewS0;
        public SoundDataSO soundDataSO;
        public UIImageSO uiImageSO;
    }

    [System.Serializable]
    public class PathConfigs
    {
        public string pathLoadFileCsv = "Localization/Localization";
    }
}