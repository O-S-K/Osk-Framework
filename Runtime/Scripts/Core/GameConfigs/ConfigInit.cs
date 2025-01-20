using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;

namespace OSK
{
    [CreateAssetMenu(fileName = "ConfigInit", menuName = "OSK/Configs/ConfigInit", order = 0)]
    public class ConfigInit : ScriptableObject
    {
        public int targetFrameRate = 60;
        public int vSyncCount = 0;
        public bool logTest = true;

        public string packageName = "";
        public string appstoreID = "";

        public bool isEncrypt = false;
        public string encryptKey = "b14ca5898a4e4133bbce2ea2315a1916";

        public DataConfigs data;
        public SettingConfigs setting;
        public PathConfigs path;

#if UNITY_EDITOR

        [Button]
        public void CreateConfig()
        {
            ListViewSO listViewS0 = CreateInstance<ListViewSO>();
            ListSoundSO listSoundSo = CreateInstance<ListSoundSO>();
            UIParticleSO uiParticleSO = CreateInstance<UIParticleSO>();


            string path = "Assets/OSK/Resources/Configs";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets/OSK/Resources", "Configs");
            }

            IOUtility.CreateDirectory(path);
            if (data.listSoundSo == null)
            {
                AssetDatabase.CreateAsset(listSoundSo, $"{path}/ListSoundSO.asset");
                data.listSoundSo = listSoundSo;
            }

            if (data.listViewS0 == null)
            {
                AssetDatabase.CreateAsset(listViewS0, $"{path}/ListViewSO.asset");
                data.listViewS0 = listViewS0;
            }

            if (data.uiParticleSO == null)
            {
                AssetDatabase.CreateAsset(uiParticleSO, $"{path}/UIParticleSO.asset");
                data.uiParticleSO = uiParticleSO;
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        [Button]
        private void AddConfigs()
        {
            packageName = Application.identifier;
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSyncCount;

            FindViewDataSOAssets();
            FindSoundDataSOAssets();
            FindParticleDataSOAssets();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
                    data.listSoundSo = v;
                }
            }
        }

        private void FindParticleDataSOAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:UIParticleSO");
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
                    data.uiParticleSO = v;
                }
            }
        }


        [Button]
        private void ShuffleEncryptKey()
        {
            encryptKey = StringUtils.ShuffleString(encryptKey);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
        public ListSoundSO listSoundSo;
        public UIParticleSO uiParticleSO;
    }

    [System.Serializable]
    public class PathConfigs
    {
        [FolderPath] public string pathLoadFileCsv = "Localization/Localization";
    }
}