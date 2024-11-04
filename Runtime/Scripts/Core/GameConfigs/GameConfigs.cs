using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class GameConfigs : GameFrameworkComponent
    {
        public ConfigInitDefault Game => game;
        [SerializeField] private ConfigInitDefault game;
        
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

}