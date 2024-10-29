using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public class RootUI : MonoBehaviour
    {
        [SerializeField] private ViewManager viewManager;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private UIMoveEffect _uiMoveEffect;
        
        public ViewManager ListViews => viewManager;
        public UIMoveEffect ParticleUI => _uiMoveEffect;
        public Canvas GetCanvas => _canvas;
        public Camera GetUICamera => _uiCamera;

        public void Initialize()
        {
            viewManager.Initialize();
        }  
    }
}