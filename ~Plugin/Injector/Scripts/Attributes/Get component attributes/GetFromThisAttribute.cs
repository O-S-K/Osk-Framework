using System;
using UnityEngine;

namespace Injector
{
    public sealed class GetFromThisAttribute : GetComponentBaseAttribute
    {
        public GetFromThisAttribute(Type exactType = null) : base(exactType)
        {
        }

        protected override void ResolveCollection(GameObject source, object targetObject, Target target)
        {
            var type = ExactType != null ? ExactType : target.MemberType;

            var resolver = GenericResolversHolder.GetResolver(type);
            resolver.GetComponents(source);
            target.SetFromList(targetObject, resolver.Objects);
        }

        protected override void ResolveSingle(GameObject source, object targetObject, Target target)
        {
            var type = ExactType != null ? ExactType : target.MemberType;
            target.Set(targetObject, source.GetComponent(type));
        }
    }
}