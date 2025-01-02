using UnityEngine;

namespace OSK
{
    public static class AppRate
    {
        private static bool _reviewRequested;

        public static void Rate()
        {
#if UNITY_IOS
            if (_reviewRequested == false)
            {
                bool popupShown = UnityEngine.iOS.Device.RequestStoreReview();
                if (popupShown)
                {
                    _reviewRequested = true;
                    Debug.Log("Review popup was presented.");
                }
                else
                {
                    Debug.Log("Review popup wasn't presented.");
                    _reviewRequested = false;
                }
            }

#elif UNITY_ANDROID
            if (_reviewRequested == false)
            {
                _reviewRequested = true;
                OpenURL();
            }
#endif
        }

        private static void OpenURL()
        {
            Main.ConfigsManager.GetLinkURL();
        }
    }
}