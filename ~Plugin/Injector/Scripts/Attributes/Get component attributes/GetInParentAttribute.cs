using System;
using UnityEngine;

namespace Injector
{
    public sealed class GetInParentAttribute : GetComponentBaseAttribute
    {
        bool includeInactive = false;

        public GetInParentAttribute()
        {
        }

        public GetInParentAttribute(Type exactType) : base(exactType)
        {
        }

        public GetInParentAttribute(bool includeInactive, Type exactType) : base(exactType)
        {
            this.includeInactive = includeInactive;
        }

        protected override void ResolveCollection(GameObject source, object targetObject, Target target)
        {
            var type = ExactType != null ? ExactType : target.MemberType;

            var resolver = GenericResolversHolder.GetResolver(type);
            resolver.GetComponentsInParent(source, includeInactive);
            target.SetFromList(targetObject, resolver.Objects);
        }

        protected override void ResolveSingle(GameObject source, object targetObject, Target target)
        {
            var type = ExactType != null ? ExactType : target.MemberType;
            var result = source.GetComponentInParent(type);

            target.Set(targetObject, result);
        }
    }
}