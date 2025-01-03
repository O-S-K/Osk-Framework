using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public class RootUI : MonoBehaviour
    {
       [SerializeField] private ViewContainer viewContainer;
       [SerializeField] private Camera _uiCamera;
       [SerializeField] private Canvas _canvas;
       [SerializeField] private UIParticle uiParticle;

        public ViewContainer ListViews => viewContainer;
        public UIParticle Particle => uiParticle;
        public Canvas GetCanvas => _canvas;
        public Camera GetUICamera => _uiCamera;

        public void Initialize()
        {
            viewContainer.Initialize();
        }
    }
}