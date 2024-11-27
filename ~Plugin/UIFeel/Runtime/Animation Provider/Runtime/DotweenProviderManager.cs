using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace OSK
{
    public class DotweenProviderManager : MonoBehaviour
    {
        public List<IDoTweenProviderBehaviours> Providers => new List<IDoTweenProviderBehaviours>(
            GetComponentsInChildren<IDoTweenProviderBehaviours>());

        public bool playOnEnable = true;
        public bool setAutoKill = true;

        public UpdateType updateType = UpdateType.Normal;
        public bool useUnscaledTime = false;
         
 
        private void Awake()
        {
            SetupSetting();
            AddProvider();
        }
        
        public void AddProvider()
        {
            var providers = GetComponentsInChildren<DoTweenBaseProvider>();
            foreach (var provider in providers)
            {
                provider.InitFromMG(playOnEnable, setAutoKill, updateType, useUnscaledTime);
            }
        }

        public void SetupSetting()
        {
            Providers.ForEach(provider =>
            {
                provider.InitFromMG(playOnEnable, setAutoKill, updateType, useUnscaledTime);
            });
        }

        public void Play() => Providers.ForEach(provider => provider.Play());
        public void Stop()=> Providers.ForEach(provider => provider.Stop());
        public void Rewind() => Providers.ForEach(provider => provider.Rewind());
        public void Preview(float time) => Providers.ForEach(provider => provider.Preview(time));
        
        public float GetTotalDuration()
        {
            float TotalDuration = 0;
            foreach(var provider in Providers)
            {
                TotalDuration += provider.GetDuration() > 0 ? provider.GetDuration() : 0;
            }
            
            Debug.Log( $"TotalDuration: {TotalDuration}");
            return TotalDuration;
        }
    }
}