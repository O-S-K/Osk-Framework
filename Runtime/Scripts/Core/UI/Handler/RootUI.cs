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

        public ViewContainer ListViews => viewContainer;
        public UIParticle Particle => uiParticle;
        public Canvas GetCanvas => _canvas;
        public CanvasScaler GetCanvasScaler => _canvasScaler;
        public Camera GetUICamera => _uiCamera;

        public void Initialize()
        {
            viewContainer.Initialize();
        } 
    }
}