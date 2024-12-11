using System;
using UnityEngine;

namespace OSK
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute
    {
    }
}