using UnityEngine;

namespace Injector
{
    public abstract class GameObjectInjectorAttribute : InjectorAttribute
    {
        public abstract bool Resolve(Target target, GameObject obj, object owner);
    }
}