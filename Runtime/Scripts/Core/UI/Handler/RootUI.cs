using System;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public class RootUI : MonoBehaviour
    {
        [SerializeField] private ViewContainer viewContainer;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private UIParticle uiParticle;
        [SerializeField] private bool dontDestroyOnLoad = true;

        public ViewContainer ListViews => viewContainer;
        public UIParticle Particle => uiParticle;
        public Canvas GetCanvas => _canvas;
        public CanvasScaler GetCanvasScaler => _canvasScaler;
        public Camera GetUICamera => _uiCamera;

        private void Awake()
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
         
        public void Initialize()
        {
            viewContainer.Initialize();
            uiParticle.Initialize();
        }
    }
}