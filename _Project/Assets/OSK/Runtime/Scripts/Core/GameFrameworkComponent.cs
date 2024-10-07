using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

namespace OSK
{
    [DefaultExecutionOrder(-998)]
    public class GameFrameworkComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Main.Register(this);
        }
    }
}