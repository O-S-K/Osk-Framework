using UnityEngine;
using System.Collections;
using System;

namespace Injector
{
    public class InjectAttribute : GameObjectInjectorAttribute, IPostBuildTargetChecker
    {
        private bool fillNulls = false;

        public InjectAttribute(bool fillNulls = false)
        {
            this.fillNulls = fillNulls;
        }

        public override bool Resolve(Target target, GameObject obj, object owner)
        {
            var currentValue = target.Get(owner);

            var ctor = target.MemberType.GetConstructor(Type.EmptyTypes);
            bool canBeCreated = ctor != null && !target.MemberType.IsAbstract;
            if (target.IsCollection)
            {
                var list = currentValue as IList;
                if (list == null)
                {
                    if (fillNulls)
                    {
                        list = Activator.CreateInstance(target.ExactMemberType) as IList;
                        target.Set(owner, list);
                    }
                    else
                        return false;
                }

                for (int index = 0; index < list.Count; index++)
                {
                    if (fillNulls && canBeCreated && list[index] == null)
                        list[index] = Activator.CreateInstance(target.MemberType);

                    if (list[index] != null)
                        Resolver.Resolve(obj, list[index]);
                }
            }
            else
            {
                if (currentValue == null)
                {
                    if (fillNulls && canBeCreated)
                    {
                        currentValue = Activator.CreateInstance(target.MemberType);
                        target.Set(owner, currentValue);
                    }
                    else
                        return false;
                }

                Resolver.Resolve(obj, currentValue);
            }

            return true;
        }

        void IPostBuildTargetChecker.CheckTarget(Target target)
        {
            if (target.MemberType.IsSubclassOf(typeof(UnityEngine.Object)))
                throw new ArgumentException("Can't inject class inherited from UnityEngine.Object!");
        }
    }
}