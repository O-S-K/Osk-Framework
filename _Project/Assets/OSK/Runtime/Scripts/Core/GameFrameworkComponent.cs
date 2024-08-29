using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFrameworkComponent : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Resister()
    {
        World.Register(this);
    }
}
