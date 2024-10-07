using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor,
        AllowMultiple = false, Inherited = true)]
    public class Inject : System.Attribute
    {
    }
}