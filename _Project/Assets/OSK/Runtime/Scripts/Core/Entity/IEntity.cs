using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    int ID { get; }
    GameObject gameObject { get; }
}
