using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class PopupResExample : Popup
{
     public override void Show()
     {
          base.Show();
          World.Time.Create(gameObject, 2, () =>
          {
               Hide();
          });
     }
}