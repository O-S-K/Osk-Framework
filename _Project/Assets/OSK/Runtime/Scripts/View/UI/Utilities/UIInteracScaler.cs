using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class UIInteracScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,IPointerExitHandler
{
    public float hoverScale = 1.1f;  
    public float defaultScale = 1f; 

    private Vector3 defaultSize;  
    public bool IsScaler = true;

    void Start()
    { 
        defaultSize = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsScaler)
            return;
        transform.localScale = defaultSize * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsScaler)
            return;
        transform.localScale = defaultSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsScaler)
            return;
        transform.localScale = defaultSize * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsScaler)
            return;
        transform.localScale = defaultSize * 1;
    }
}
