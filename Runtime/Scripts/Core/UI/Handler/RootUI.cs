using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public class RootUI : MonoBehaviour
    {
        [FindChild("ViewManager", ComponentType.Component)] [SerializeField] private ViewContainer viewContainer;
        [FindChild("UICamera", ComponentType.Component)]    [SerializeField] private Camera _uiCamera;
        [FindChild("Canvas", ComponentType.Component)]      [SerializeField] private Canvas _canvas;
        [FindChild("UIParticles", ComponentType.Component)] [SerializeField] private UIParticle uiParticle;

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