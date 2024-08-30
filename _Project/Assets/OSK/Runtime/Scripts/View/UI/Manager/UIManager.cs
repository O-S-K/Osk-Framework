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
    

    public T ShowScreen<T>() where T : UIScreen
    {
        return _screenManager.Show<T>();
    }

    public T GetScreen<T>() where T : UIScreen
    {
        return _screenManager.GetScreen<T>();
    }
    
    public T ShowPopupFormRes<T>(string path) where T : Popup
    {
        return _popupController.Create<T>(path);
    }
    
    public void DeletePopup<T>(T popup) where T : Popup
    {
        _popupController.Delete<T>(popup);
    }

    public T ShowPopup<T>() where T : Popup
    {
        return _popupController.Show<T>();
    }

    public T GetPopup<T>() where T : Popup
    {
        return _popupController.Get<T>();
    }
}