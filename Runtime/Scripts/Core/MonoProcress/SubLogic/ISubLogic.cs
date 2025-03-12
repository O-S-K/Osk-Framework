using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public interface ISubUpdate { void Tick(); }
    public interface ISubFixedUpdate { void FixedTick(); }
    public interface ISubLateUpdate { void LateTick(); }
}
