using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FindChildAttribute : Attribute
{
    public string name;
    public ComponentType componentType;

    public FindChildAttribute(string name, ComponentType componentType)
    {
        this.name = name;
        this.componentType = componentType;
    }
}