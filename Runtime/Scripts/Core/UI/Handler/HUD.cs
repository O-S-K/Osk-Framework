using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private ScreenManager _screenManager;
        [SerializeField] private PopupManager _popupManager;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private UIMoveEffect _uiMoveEffect;
        
        public ScreenManager GetScreenManager => _screenManager;
        public PopupManager GetPopupManager => _popupManager;
        public UIMoveEffect ParticleUI => _uiMoveEffect;
        public Canvas GetCanvas => _canvas;
        public Camera GetUICamera => _uiCamera;

        private void Awake()
        {
            GetScreenManager.Initialize();
            GetPopupManager.Initialize();
        }
    }
}