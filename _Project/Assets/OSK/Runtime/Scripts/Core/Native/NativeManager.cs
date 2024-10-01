using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;


namespace OSK
{
    public class NativeManager : GameFrameworkComponent
    {
        public void Vibrate(EffectHaptic effectType)
        {
            if (GameData.Vibration == false)
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