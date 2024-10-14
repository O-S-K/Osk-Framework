using UnityEngine;
using System.Collections;
using System;

namespace Injector
{
    public class LoadFromResourcesAttribute : ObjectIndependentInjectorAttribute, IPostBuildTargetChecker
    {
        private string loadPath = "";
        private Type exactType;

        public LoadFromResourcesAttribute(Type exactType, string loadPath = "")
        {
            this.exactType = exactType;
            this.loadPath = loadPath;
        }

        public LoadFromResourcesAttribute(string loadPath = "")
        {
            this.loadPath = loadPath;
        }

        public override void Resolve(Target target, object targetObject)
        {
            var type = exactType == null ? target.MemberType : exactType;

            if (target.IsCollection)
            {
                target.SetFromList(targetObject, Resources.LoadAll(loadPath, type), true);
            }
            else
            {
                if (string.IsNullOrEmpty(loadPath))
                {
                    var found = Resources.LoadAll(string.Empty, type);
                    target.Set(targetObject, found.Length == 0 ? null : found[0]);
                }
                else
                    target.Set(targetObject, Resources.Load(loadPath, type));
            }
        }

        void IPostBuildTargetChecker.CheckTarget(Target target)
        {
            if (target.MemberType != typeof(UnityEngine.Object) && !target.MemberType.IsSubclassOf(typeof(UnityEngine.Object)))
                throw new ArgumentException("Target should be UnityEngine.Object or derived from it");

            if (exactType != null && !target.MemberType.IsAssignableFrom(exactType))
                throw new ArgumentException("Impossible to load resource of type \"" + exactType.Name + "\" to field of type \"" + target.MemberType.Name + "\" with name \"" + target.Member.Name + "\" declared in type \"" + target.Member.DeclaringType.Name + "\"");
        }
    }
}
