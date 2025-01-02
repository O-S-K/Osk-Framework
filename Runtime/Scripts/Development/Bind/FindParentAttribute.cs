using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FindParentAttribute : Attribute
{
    public string name;
    public ComponentType componentType;
    public bool overrideExisting;


    public FindParentAttribute(string name, ComponentType componentType, bool overrideExisting = false)
    {
        this.name = name;
        this.componentType = componentType;
        this.overrideExisting = overrideExisting;
    }
}