using System;
using UnityEngine;

namespace OSK
{
    public class GameConfigsManager : GameFrameworkComponent
    {
        public ConfigInit init {get; private set;}
         
        public string VersionApp => Application.version;

        
        public override void OnInit()
        {
            if (Main.Instance.configInit == null)
            {
                Logg.LogError("Not found ConfigInit in Main");
                return;
            }
            init = Main.Instance.configInit;
        }

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

        public string GetUrlAppstore => "https://apps.apple.com/app/id" + init.appstoreID;
        public string GetUrlGooglePlay => "https://play.google.com/store/apps/details?id=" + init.packageName;

        public void GetLinkURL()
        {
#if UNITY_ANDROID
            Application.OpenURL(GetUrlGooglePlay);
#elif UNITY_IOS
            Application.OpenURL(GetUrlAppstore);
#endif
        }
    }
}