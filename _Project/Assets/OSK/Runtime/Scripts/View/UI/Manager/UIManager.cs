using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class UIManager : GameFrameworkComponent
{
    [SerializeField] private ScreenManager _screenManager;
    [SerializeField] private PopupManager _popupController;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private UIMoveEffect _uiMoveEffect;
    
    public UIMoveEffect ParticleUI => _uiMoveEffect;

    
    protected override void Awake()
    {
        base.Awake();
        _screenManager.Initialize();
        _popupController.Initialize();
    }
    

    public Camera GetUICamera()
    {
        return _uiCamera;
    }
    
    public Canvas GetCanvas()
    {
        return _canvas;
    }


    public T ShowScreen<T>() where T : UIScreen
    {
        return _screenManager.Show<T>();
    }

    public T GetScreen<T>() where T : UIScreen
    {
        return _screenManager.GetScreen<T>();
    }

    public T ShowPopup<T>(string path, bool isHidePrevPopup = true) where T : Popup
    {
        return _popupController.Create<T>(path, isHidePrevPopup);
    }

    public void DeletePopup<T>(T popup) where T : Popup
    {
        _popupController.Delete<T>(popup);
    }

    public T ShowPopup<T>(bool isHidePrevPopup = true) where T : Popup
    {
        return _popupController.Show<T>(isHidePrevPopup);
    }
    
    public T ShowOverlayPopup<T>(string path) where T : Popup
    {
        return _popupController.ShowOverlay<T>(path);
    }

    public T GetPopup<T>() where T : Popup
    {
        return _popupController.Get<T>();
    }

    public T ShowPopupAny<T>() where T : Popup
    {
        return _popupController.ShowAny<T>();
    }
}