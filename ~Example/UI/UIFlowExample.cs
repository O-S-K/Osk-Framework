using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using OSK;
using UnityEngine;

public class UIFlowExample : MonoBehaviour
{
    private void Start()
    {
        var homeUI = Main.UI.Spawn<HomeUIExample>("Popups/PopupHome");
        homeUI.Open();
    } 
}
