
using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class IngameUIExample  : UIScreen
{
    public override void Initialize()
    {
        Debug.Log("IngameUIExample Initialize");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //World.UI.ShowPopup<WinPopupExample>();
            World.UI.ShowPopupFormRes<WinPopupExample>("Popups/PopupWin");
        }
    }

    public void BackHome()
    {
        World.UI.ShowScreen<HomeUIExample>();
    }
}

