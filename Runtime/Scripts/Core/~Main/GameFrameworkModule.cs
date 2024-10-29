using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    internal abstract class GameFrameworkModule
    {
        internal virtual int Priority => 0;
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        internal abstract void Shutdown();
    }
}
