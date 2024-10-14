using System;

namespace Injector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class InjectorAttribute : Attribute
    {
    }
}