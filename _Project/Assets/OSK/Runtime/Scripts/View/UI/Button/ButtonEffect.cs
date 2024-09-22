using System;
using CustomInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    
    public Action OnClick;
    
    [SerializeField] private float hoverScale = 0.9f;
    [SerializeField] private float defaultScale = 1f;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease ease = Ease.OutBounce;

    [SerializeField] private bool _isScaler = true;
    private Vector3 defaultSize;

    [SerializeField] private bool playSoundOnClick;

    [ShowIf(nameof(playSoundOnClick)), SerializeField]
    private string soundName;

    private void OnEnable()
    {
        defaultSize = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playSoundOnClick)
            World.Sound.Play(soundName, false);

        if (_isScaler)
            transform.DOScale(defaultSize * hoverScale, duration).SetEase(ease);
        
        // OnClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isScaler)
            transform.DOScale(defaultSize, duration).SetEase(ease);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if (_isScaler)
        //     transform.DOScale(defaultSize * hoverScale, duration).SetEase(ease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (_isScaler)
        //     transform.DOScale(defaultSize, duration).SetEase(ease);
    }
}