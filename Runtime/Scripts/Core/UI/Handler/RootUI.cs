using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

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
        
        [SerializeField] private bool isPortrait = true;
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
        
        [Button]
        private void SetupCanvas()
        {
#if UNITY_EDITOR
            _canvas.referencePixelsPerUnit = 100;
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    
            if (isPortrait)
            {
                _canvasScaler.referenceResolution = new Vector2(1080, 1920);
                _canvasScaler.matchWidthOrHeight = 0;
            }
            else
            {
                _canvasScaler.referenceResolution = new Vector2(1920, 1080);
                _canvasScaler.matchWidthOrHeight = 1;
            }
            UnityEditor.EditorUtility.SetDirty(_canvas);
#endif
        }
    }
}