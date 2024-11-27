using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
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
        
        public List<DoTweenBaseProvider> providers = new List<DoTweenBaseProvider>();


        private void Awake()
        {
            SetupSetting();
            providers = new List<DoTweenBaseProvider>(GetComponentsInChildren<DoTweenBaseProvider>());
        }
        
        public void AddProvider()
        {
            providers = new List<DoTweenBaseProvider>(GetComponentsInChildren<DoTweenBaseProvider>());
        }

        public void SetupSetting()
        {
            
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