using System;
using UnityEngine;

public enum ComponentType
{
    Button,
    Text,
    TextTMPro,
    Image,
    Slider,
    InputField,
    Script,
    Rigidbody,
    Collider,
    GameObject
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FindAttribute : Attribute
{
    public string name;
    public ComponentType componentType;

    public FindAttribute(string name, ComponentType componentType)
    {
        this.name = name;
        this.componentType = componentType;
    }
}