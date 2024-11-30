using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public interface IEntity
    {
        int ID { get; }
        void Tick();
        void FixedTick();
        void LateTick();
        T Add<T>() where T : EComponent;
        T Get<T>() where T : EComponent;
        void Remove<T>() where T : EComponent;
    }
}