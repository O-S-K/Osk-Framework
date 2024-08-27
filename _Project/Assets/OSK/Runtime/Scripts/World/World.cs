using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
}
