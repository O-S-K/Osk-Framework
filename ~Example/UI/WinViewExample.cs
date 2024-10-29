using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class WinViewExample : View
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Main.UI.Open<ViewResExample>();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Main.UI.Delete(this);
        }
    }
 
}
