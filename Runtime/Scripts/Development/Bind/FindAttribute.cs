using System;
using UnityEngine;

public enum ComponentType
{
    Selectable,
    Graphic, 
    Component,
    GameObject
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FindAttribute : Attribute
{
    public string name;
    public ComponentType componentType;
    public bool overrideExisting;


    public FindAttribute(string name, ComponentType componentType, bool overrideExisting = false)
    {
        this.name = name;
        this.componentType = componentType;
        this.overrideExisting = overrideExisting;
    }
}