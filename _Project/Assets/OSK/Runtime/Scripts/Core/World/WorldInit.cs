using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        InitComponents();
    }
}
