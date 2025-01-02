using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FindParentAttribute : Attribute
{
    public string name;
    public ComponentType componentType;

    public FindParentAttribute(string name, ComponentType componentType)
    {
        this.name = name;
        this.componentType = componentType;
    }
}