using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class HomeUIExample : UIScreen
{
    public override void Initialize()
    {
        Debug.Log("HomeUIExample Initialize");
    }
    
    public void StartIngame()
    {
        World.UI.ShowScreen<IngameUIExample>();
    }
}
