using System;
using UnityEngine;

namespace Injector
{
    public sealed class GetInChildrenAttribute : GetComponentBaseAttribute
    {
        private bool includeInactive;

        public GetInChildrenAttribute()
        {
        }

        public GetInChildrenAttribute(Type exactType) : base(exactType)
        {
        }

        public GetInChildrenAttribute(bool includeInactive, Type exactType) : base(exactType)
        {
            this.includeInactive = includeInactive;
        }

        protected override void ResolveCollection(GameObject source, object targetObject, Target target)
        {
            var type = ExactType != null ? ExactType : target.MemberType;

            var resolver = GenericResolversHolder.GetResolver(type);
            resolver.GetComponentsInChildren(source, includeInactive);
            target.SetFromList(targetObject, resolver.Objects);
        }

        protected override void ResolveSingle(GameObject source, object targetObject, Target target)
        {
            var result = source.GetComponentInChildren(ExactType != null ? ExactType : target.MemberType);
            target.Set(targetObject, result);
        }
    }
}