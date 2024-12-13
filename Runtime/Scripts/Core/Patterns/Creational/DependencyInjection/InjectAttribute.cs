using System;
using UnityEngine;

namespace OSK
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : PropertyAttribute
    {
        public string Key { get; }

        public InjectAttribute(string key = null)
        {
            Key = key;
        }
    }
}