using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class PopupResExample : Popup
{
    public override void Show()
    {
        base.Show();
        Main.Time.Create(gameObject, 1, false, () => { Hide(); });
    }
}