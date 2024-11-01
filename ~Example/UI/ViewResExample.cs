using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class ViewResExample : View
{
    public override void Open(object data = null)
    {
        base.Open(data);
        // Main.Time.Create(gameObject, 1, false, () =>
        // {
        //     Hide();
        // });
    }
}