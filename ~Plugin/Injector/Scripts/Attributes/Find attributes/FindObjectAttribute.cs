using System;
using UnityEngine;

namespace Injector
{
    public class FindObjectAttribute : ObjectIndependentInjectorAttribute, IPostBuildTargetChecker
    {
        Type exactType;
        public Type ExactType
        {
            get
            {
                return exactType;
            }
        }

        public FindObjectAttribute(Type exactType = null)
        {
            this.exactType = exactType;
        }

        public override void Resolve(Target target, object targetObject)
        {
            var type = exactType != null ? exactType : target.MemberType;

            if (target.IsCollection)
            {
                var found = GameObject.FindObjectsOfType(type);
                target.SetFromList(targetObject, found, true);
            }
            else
                target.Set(targetObject, GameObject.FindObjectOfType(type));
        }

        void IPostBuildTargetChecker.CheckTarget(Target target)
        {
            if (target.MemberType != typeof(UnityEngine.Object) && !target.MemberType.IsSubclassOf(typeof(UnityEngine.Object)))
                throw new ArgumentException("Target should be UnityEngine.Object or derived from it");

            var type = exactType != null ? exactType : target.MemberType;
            if (!target.MemberType.IsAssignableFrom(type))
                throw new ArgumentException("Can't assign \"" + type.Name + "\" to \"" + target.Member.Name + "\" of type \"" + target.MemberType.Name + "\" in declared in class \"" + target.MemberType.DeclaringType.Name + "\"");
        }
    }
}