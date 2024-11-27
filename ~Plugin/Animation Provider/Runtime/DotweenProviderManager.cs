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
        
        public List<DoTweenBaseProvider> providers = new List<DoTweenBaseProvider>();


        private void Awake()
        {
            SetupSetting();
            AddProvider();
        }
        
        public void AddProvider()
        {
            providers = new List<DoTweenBaseProvider>(GetComponentsInChildren<DoTweenBaseProvider>());
        }

        public void SetupSetting()
        {
            providers.ForEach(provider =>
            {
                provider.InitFromMG(playOnEnable, setAutoKill, updateType, useUnscaledTime);
            });
        }

        public void Play() => providers.ForEach(provider => provider.Play());
        public void Stop()=> providers.ForEach(provider => provider.Stop());
        public void Rewind() => providers.ForEach(provider => provider.Rewind());
        public void Preview(float time) => providers.ForEach(provider => provider.Preview(time));
        
        public float GetCurrentDuration()
        {
            float CurrentDuration = 0;
            foreach(var provider in providers)
            {
                CurrentDuration += provider.GetCurrentDuration() > 0 ? provider.GetCurrentDuration() : 0;
            }
            
            Debug.Log( $"CurrentDuration: {CurrentDuration}");
            return CurrentDuration;
        }
        
        public float GetTotalDuration()
        {
            float TotalDuration = 0;
            foreach(var provider in providers)
            {
                TotalDuration += provider.GetDuration() > 0 ? provider.GetDuration() : 0;
            }
            
            Debug.Log( $"TotalDuration: {TotalDuration}");
            return TotalDuration;
        }
    }
}