using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class HomeUIExample : View
{
    public override void Open(object data = null)
    {
        base.Open(data);
        Debug.Log("HomeUIExample OnShow");
    }

    public override void Hide()
    {
        base.Hide();
        Debug.Log("HomeUIExample OnHide");
    }

    public void OnClickButton()
    {
        Debug.Log("HomeUIExample OnClickButton");
    }
}
