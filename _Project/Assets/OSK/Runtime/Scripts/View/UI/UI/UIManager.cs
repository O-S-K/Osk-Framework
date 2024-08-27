using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScreenManager _screenManager;
    [SerializeField] private PopupManager _popupController;

    private void Awake()
    {
        _screenManager.Initialize();
        _popupController.Initialize();
    }

    public T ShowScreen<T>() where T : UIScreen
    {
        var screen = _screenManager.GetScreen<T>();
        return screen;
    }

    public T GetScreen<T>() where T : UIScreen
    {
        return _screenManager.GetScreen<T>();
    }
    

    public T ShowPopup<T>() where T : Popup
    {
        return _popupController.Show<T>();
    }

    public T GetPopup<T>() where T : Popup
    {
        return _popupController.GetPopup<T>();
    }
}