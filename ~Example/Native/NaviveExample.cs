using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class NaviveExample : MonoBehaviour
    {
        [Button]
        public void CaptureScreenshot()
        {
            Main.Native.CaptureScreenshot();
        }

        [Button]
        public void Rating()
        {
            Main.Native.Rating();
        }

        [Button]
        public void Vibrate(EffectHaptic effectType)
        {
            Main.Native.Vibrate(effectType);
        }
    }

}