using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class WinPopupExample : Popup
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            World.UI.ShowPopup<PopupResExample>();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            World.UI.DeletePopup(this);
        }
    }
 
}
