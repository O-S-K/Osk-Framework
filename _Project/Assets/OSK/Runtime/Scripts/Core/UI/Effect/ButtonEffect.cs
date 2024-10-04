﻿using System;
using CustomInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    public Action OnClick;
    public bool IsPressed { get; private set; }

    [Header("Root Transform")] public Transform rootAppear;
    public float scaleInit = 0f;


    [Header("Animation")] [SerializeField] private float hoverScale = 0.9f;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease ease = Ease.OutBounce;

    [HideIf(nameof(_isRotator)), HideIf(nameof(_isShaker)), SerializeField]
    private bool _isScaler = true;

    private Vector3 defaultSize;

    [HideIf(nameof(_isScaler)), HideIf(nameof(_isShaker)), SerializeField]
    private bool _isRotator;

    private Vector3 defaultRotation;

    [HideIf(nameof(_isScaler)), HideIf(nameof(_isRotator)), SerializeField]
    private bool _isShaker;

    private Vector3 defaultPosition;

    [SerializeField] private bool playSoundOnClick;

    [ShowIf(nameof(playSoundOnClick)), SerializeField]
    private string soundName;

    private Transform root => rootAppear ? rootAppear : transform;

    private void OnEnable()
    {
        defaultSize = Vector3.one;
        defaultRotation = Vector3.zero;
        defaultPosition = transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(GetComponent<Button>() != null  && GetComponent<Button>().interactable == false)
            return;
            
        //if (playSoundOnClick)
            Main.Sound.Play("ui_click", false);

        if (_isScaler)
            root.DOScale(defaultSize * hoverScale, duration).SetEase(ease);

        if (_isRotator)
            root.DORotate(defaultRotation + new Vector3(0, 0, 10), duration).SetEase(ease);

        if (_isShaker)
            root.DOShakePosition(duration, 10, 10, 90, false);

        IsPressed = true;
        // OnClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isScaler)
            root.DOScale(defaultSize, duration).SetEase(ease);

        if (_isRotator)
            root.DORotate(defaultRotation, duration).SetEase(ease);

        if (_isShaker)
            root.DOLocalMove(defaultPosition, duration).SetEase(ease);
        
        IsPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if (_isScaler)
        //     root.DOScale(defaultSize * hoverScale, duration).SetEase(ease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (_isScaler)
        //     root.DOScale(defaultSize, duration).SetEase(ease);
    }
}