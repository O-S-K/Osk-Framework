using System;
using UnityEngine;

namespace Injector
{
    public class FindObjectByTagAttribute : ObjectIndependentInjectorAttribute, IPostBuildTargetChecker
    {
        string tag;
        
        public FindObjectByTagAttribute(string tag)
        {
            this.tag = tag;
        }

        public override void Resolve(Target target, object targetObject)
        {
            if (target.IsCollection)
            {
                var found = GameObject.FindGameObjectsWithTag(tag);
                target.SetFromList(targetObject, found, true);
            }
            else
                target.Set(targetObject, GameObject.FindGameObjectWithTag(tag));
        }

        void IPostBuildTargetChecker.CheckTarget(Target target)
        {
            if (target.MemberType != typeof(UnityEngine.Object) && !target.MemberType.IsSubclassOf(typeof(UnityEngine.Object)))
                throw new ArgumentException("Target should be UnityEngine.Object or derived from it");

            if (!target.MemberType.IsAssignableFrom(typeof(GameObject)))
                throw new ArgumentException("Can't assign game object to field of different type");
        }
    }
}