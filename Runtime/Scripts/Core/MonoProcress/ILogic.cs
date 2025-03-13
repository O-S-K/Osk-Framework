using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public interface IUpdate { void Tick(float deltaTime); }
    public interface IFixedUpdate { void FixedTick(float fixedDeltaTime); }
    public interface ILateUpdate { void LateTick(float deltaTime); }
}
