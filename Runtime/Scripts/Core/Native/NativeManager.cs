using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

namespace OSK
{
    public class NativeManager : GameFrameworkComponent
    {
        public override void OnInit()
        { 
        }
        
        public void CaptureScreenshot()
        {
            ScreenShot.CaptureScreenshot(this);
        }

        public void Rating()
        {
            OSK.AppRate.Rate();
        }

        public void Vibrate(EffectHaptic effectType)
        {
            if (PlayerPrefs.GetInt("Vibration") == 0)
                return;

            switch (effectType)
            {
                case EffectHaptic.Light:
                    HapticFeedback.LightFeedback();
                    break;
                case EffectHaptic.Medium:
                    HapticFeedback.MediumFeedback();
                    break;
                case EffectHaptic.Heavy:
                    HapticFeedback.HeavyFeedback();
                    break;
            }
        }
    }
}