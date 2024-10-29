using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class GameConfigs : GameFrameworkComponent
    {
        public int targetFrameRate = 60;
        public int vSyncCount = 0;

        public DataConfigs data;
        public Settings setting;
        public PathConfigs path;

        public string VersionApp => Application.version;

        public void CheckVersion(Action onNewVersion)
        {
            string currentVersion = VersionApp;
            string key = "lastVersion";

            if (PlayerPrefs.HasKey(key))
            {
                string savedVersion = PlayerPrefs.GetString(key);
                if (currentVersion != savedVersion)
                {
                    // New version
                    onNewVersion?.Invoke();
                }
                else {  } // No new version
            }
            else {  } // First time

            PlayerPrefs.SetString(key, currentVersion);
            PlayerPrefs.Save();
        }

        public string GetUrlAppstore => "https://apps.apple.com/app/id" + Application.identifier;
        public string GetUrlGooglePlay => "https://play.google.com/store/apps/details?id=" + Application.identifier;

        public void GetLinkURL()
        {
#if UNITY_ANDROID
            Application.OpenURL(GetUrlGooglePlay);
#elif UNITY_IOS
            Application.OpenURL(UrlIOS);
#endif
        }

        public override void OnInit()
        {
            
        }
    }

    [System.Serializable]
    public class Settings
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