using UnityEngine;

public class CanvasScaleHandler : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private UnityEngine.UI.CanvasScaler canvasScaler;
    
    [SerializeField] private bool isPortrait;
    private void Awake()
    {
        float currentRatio = isPortrait ? 1080f / 1920 : 1920f / 1080;
        float newRatio = (float) Screen.width / Screen.height;
        SetupCanvasScaler(newRatio);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        if (camera != null && canvasScaler != null)
        {
            float currentRatio = isPortrait ? 1080f / 1920 : 1920f / 1080;
            float newRatio = (float) camera.pixelWidth / camera.pixelHeight;
            SetupCanvasScaler(newRatio);
        }
    }

    private void SetupCanvasScaler(float ratio)
    {
        canvasScaler.matchWidthOrHeight = ratio > .65f ? 1 : 0;
    }
}
