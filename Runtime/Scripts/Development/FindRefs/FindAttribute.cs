using System;
using UnityEngine;

namespace OSK
{
    public enum EFindType
    {
        Self,
        Child,
        Parent,
        Scene,
        Sibling
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FindAttribute : Attribute
    {
        public string name { get; }
        public Type type { get; }
        public EFindType findType { get; }

        public FindAttribute(string name, EFindType findType = EFindType.Self)
        {
            this.name = name;
            this.findType = findType; 
        }

        public FindAttribute(Type type, EFindType findType = EFindType.Self)
        {
            this.type = type;
            this.findType = findType;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FindsAttribute : Attribute
    {
        public EFindType findType;
        public string name;

        public FindsAttribute(EFindType findType = EFindType.Self, string name = "")
        {
            this.findType = findType;
            this.name = name;
        } 
    }
}