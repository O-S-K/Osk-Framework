using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace OSK
{
    public class DotweenProviderManager : MonoBehaviour
    {
        public List<IDoTweenProviderBehaviours> Providers => new List<IDoTweenProviderBehaviours>(
            GetComponentsInChildren<IDoTweenProviderBehaviours>());
        
        public bool playOnEnable = true;
        public bool setAutoKill = true;
        public float delay = 0f;

        public UpdateType updateType = UpdateType.Normal;
        public bool useUnscaledTime = false;

        
        private void Awake()
        {
            SetupSetting();
        }

        public void SetupSetting()
        {
            Providers.ForEach(provider =>
            {
                provider.settings.playOnEnable = playOnEnable;
                provider.settings.setAutoKill = setAutoKill;
                provider.settings.delay += delay;
                provider.settings.updateType = updateType;
                provider.settings.useUnscaledTime = useUnscaledTime;
            });
        }


        public void Play()
        {
            Providers.ForEach(provider => provider.Play());
        }
        
        public void Stop()
        {
            Providers.ForEach(provider => provider.Stop());
        } 
        
        public bool Rewind()
        {
            Providers.ForEach(provider => provider.Rewind());
            return true;
        }
        
        public void Preview(float time)
        {
            Providers.ForEach(provider => provider.Preview(time));
        } 
    }
}