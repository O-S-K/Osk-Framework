using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class GameFrameworkComponent : MonoBehaviour
{
    protected virtual void Awake()
    {
        World.Register(this);
    }
}