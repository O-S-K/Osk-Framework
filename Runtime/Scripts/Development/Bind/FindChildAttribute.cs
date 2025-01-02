using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FindChildAttribute : Attribute
{
    public string name;
    public ComponentType componentType;
    public bool overrideExisting;


    public FindChildAttribute(string name, ComponentType componentType, bool overrideExisting = false)
    {
        this.name = name;
        this.componentType = componentType;
        this.overrideExisting = overrideExisting;
    }
}