namespace Injector
{
    public abstract class ObjectIndependentInjectorAttribute : InjectorAttribute
    {
        public abstract void Resolve(Target target, object targetObject);
    }
}