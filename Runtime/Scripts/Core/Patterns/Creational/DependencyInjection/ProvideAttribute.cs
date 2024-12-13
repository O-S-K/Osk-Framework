using System;
using UnityEngine;

namespace OSK
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute
    {
        public string Key { get; }

        public ProvideAttribute(string key = null)
        {
            Key = key;
        }
    }
}