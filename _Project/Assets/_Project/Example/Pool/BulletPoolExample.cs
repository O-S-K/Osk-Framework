using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolExample : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(Release), 1);
    }
    
    
    private void Release()
    {
        World.Pool.Despawn(this);
    }
}
