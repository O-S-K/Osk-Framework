
using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class IngameUIExample  : UIScreen
{
    public override void Initialize()
    {
        Debug.Log("Ingame UIExample Initialize");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //World.UI.ShowPopup<WinPopupExample>();
            Main.UI.ShowPopup<WinPopupExample>("Popups/PopupWin");
        }
    }

    public void BackHome()
    {
        Main.UI.ShowScreen<HomeUIExample>();
    }
}

