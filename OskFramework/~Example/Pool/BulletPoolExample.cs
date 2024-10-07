using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class BulletPoolExample : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(Release), 1);
    }
    
    
    private void Release()
    {
        Main.Pool.Despawn(this);
    }
}
