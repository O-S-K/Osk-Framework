using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void Vibrate()
        {
            if (PlayerPrefs.GetInt("Vibration") == 0)
                return;
             
        }
    }
}