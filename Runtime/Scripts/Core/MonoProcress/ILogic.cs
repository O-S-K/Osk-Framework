using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public interface IUpdate { void Tick(); }
    public interface IFixedUpdate { void FixedTick(); }
    public interface ILateUpdate { void LateTick(); }
}
