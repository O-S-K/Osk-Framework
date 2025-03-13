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
        private void ShuffleEncryptKey()
        {
            encryptKey = StringUtils.Shuffle(encryptKey);
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