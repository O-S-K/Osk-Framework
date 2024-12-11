using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public abstract class ContextInstaller : MonoBehaviour
    { 
        protected virtual void Awake() => InstallBindings();
        public abstract void InstallBindings();
    }
}
