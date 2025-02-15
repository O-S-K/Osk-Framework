using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public abstract class GameInit : GameFrameworkComponent
    {
        public override void OnInit()
        {
            BindData();
        }

        public abstract void BindData();
    }
}
