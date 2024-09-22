using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    int ID { get; }
    GameObject gameObject { get; }
    void OnTick();
    T Add<T>() where T : EComponent;
    T Get<T>() where T : EComponent;
}
