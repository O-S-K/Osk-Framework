using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OSK
{
    public partial class Main
    {
        public bool isDestroyingOnLoad = false;

        private void Awake()
        {
            if (isDestroyingOnLoad)
                DontDestroyOnLoad(gameObject);
            InitComponents();
        } 
    }
}