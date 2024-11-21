using System;
using System.Diagnostics;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
[Conditional("UNITY_EDITOR")]
public class PreviewSpriteAttribute : PropertyAttribute
{ 
}