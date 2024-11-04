using UnityEngine;

namespace OSK
{
    [CreateAssetMenu(fileName = "ConfigInitDefault", menuName = "OSK/Configs/ConfigInitDefault", order = 0)]
    public class ConfigInitDefault : ScriptableObject
    {
        public int targetFrameRate = 60;
        public int vSyncCount = 0;

        public DataConfigs data;
        public SettingConfigs setting;
        public PathConfigs path;
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
        public SoundDataSO soundDataSo;
    }
    
    [System.Serializable]
    public class PathConfigs
    { 
        public string pathLoadFileCsv = "Localization/Localization";
    }
}