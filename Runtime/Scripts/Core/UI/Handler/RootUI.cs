using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public class RootUI : MonoBehaviour
    {
        [SerializeField] private ViewManager _viewManager;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private UIParticle uiParticle;
        
        public ViewManager ListViews => _viewManager;
        public UIParticle Particle => uiParticle;
        public Canvas GetCanvas => _canvas;
        public Camera GetUICamera => _uiCamera;

        public void Initialize()
        {
            _viewManager.Initialize();
        } 
    }
}