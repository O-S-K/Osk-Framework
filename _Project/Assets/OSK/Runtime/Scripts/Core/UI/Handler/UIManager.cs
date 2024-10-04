using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class UIManager : GameFrameworkComponent
{
    private HUD _hud;
    public UIMoveEffect ParticleUI =>  _hud.ParticleUI;
    public Canvas GetCanvas =>  _hud.GetCanvas;
    public Camera GetUICamera =>  _hud.GetUICamera;


    protected override void Awake()
    {
        base.Awake();
        
        if(_hud == null)
            _hud = FindObjectOfType<HUD>();
        _hud.GetScreenManager.Initialize();
        _hud.GetPopupManager.Initialize();
    }

  
    #region Screens
    public T ShowScreen<T>() where T : UIScreen
    {
        return _hud.GetScreenManager.Show<T>();
    }

    public void ShowScreen(UIScreen screen)
    {
        _hud.GetScreenManager.Show(screen);
    }

    public bool IsScreenShowing(UIScreen screen)
    {
        return _hud.GetScreenManager.GetScreen(screen).IsShowing;
    }

    public void HideScreen(UIScreen screen)
    {
        _hud.GetScreenManager.Hide(screen);
    }

    public T GetScreen<T>() where T : UIScreen
    {
        return _hud.GetScreenManager.GetScreen<T>();
    }

    public List<UIScreen> GetAllScreens()
    {
        return _hud.GetScreenManager.GetAllScreens();
    }

    #endregion

    #region Popups

    public T ShowPopup<T>(string path, bool isHidePrevPopup = true) where T : Popup
    {
        return _hud.GetPopupManager.Create<T>(path, isHidePrevPopup);
    }

    public void DeletePopup<T>(T popup) where T : Popup
    {
        _hud.GetPopupManager.Delete<T>(popup);
    }

    public T ShowPopup<T>(bool isHidePrevPopup = true) where T : Popup
    {
        return _hud.GetPopupManager.Show<T>(isHidePrevPopup);
    }


    public T ShowPopupAny<T>() where T : Popup
    {
        return _hud.GetPopupManager.ShowAny<T>();
    }

    public void ShowPopup(Popup popup)
    {
        _hud.GetPopupManager.Show(popup, true);
    }

    public void HidePopup(Popup popup)
    {
        _hud.GetPopupManager.Hide(popup);
    }

    public T ShowOverlayPopup<T>(string path) where T : Popup
    {
        return _hud.GetPopupManager.ShowOverlay<T>(path);
    }

    public T GetPopup<T>() where T : Popup
    {
        return _hud.GetPopupManager.Get<T>();
    }

    public bool IsPopupShowing(Popup popup)
    {
        return _hud.GetPopupManager.Get(popup).IsShowing;
    }

    public List<Popup> GetAllPopups()
    {
        return _hud.GetPopupManager.GetAllPopups();
    }

    #endregion
}