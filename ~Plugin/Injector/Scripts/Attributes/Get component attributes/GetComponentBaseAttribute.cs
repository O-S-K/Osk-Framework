using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Injector
{
    public abstract class GetComponentBaseAttribute : GameObjectInjectorAttribute, IPostBuildTargetChecker
    {
        protected static List<Component> components = new List<Component>();
        
        protected interface IGenericResolver
        {
            IList Objects
            {
                get;
            }

            void GetComponents(GameObject source);
            void GetComponentsInChildren(GameObject source, bool includeInactive);
            void GetComponentsInParent(GameObject source, bool includeInactive);
        }

        protected class GenericResolverResolver<T> : IGenericResolver
        {
            List<T> list = new List<T>();

            IList IGenericResolver.Objects
            {
                get
                {
                    return list;
                }
            }

            void IGenericResolver.GetComponents(GameObject source)
            {
                source.GetComponents<T>(list);
            }

            void IGenericResolver.GetComponentsInChildren(GameObject source, bool includeInactive)
            {
                source.GetComponentsInChildren<T>(includeInactive, list);
            }

            void IGenericResolver.GetComponentsInParent(GameObject source, bool includeInactive)
            {
                source.GetComponentsInParent<T>(includeInactive, list);
            }
        }

        protected static class GenericResolversHolder
        {
            static Type[] oneArgument = new Type[] { null };

            static Dictionary<Type, IGenericResolver> resolvers = new Dictionary<Type, IGenericResolver>();

            public static IGenericResolver GetResolver(Type type)
            {
                IGenericResolver resolver = null;
                if (!resolvers.TryGetValue(type, out resolver))
                {
                    var generic = typeof(GenericResolverResolver<>);

                    oneArgument[0] = type;
                    var createdGenericType = generic.MakeGenericType(oneArgument);

                    resolver = Activator.CreateInstance(createdGenericType) as IGenericResolver;
                    resolvers.Add(type, resolver);
                }

                return resolver;
            }
        }

        Type exactType;
        public Type ExactType
        {
            get
            {
                return exactType;
            }
        }
        
        public GetComponentBaseAttribute(Type exactType = null)
        {
            this.exactType = exactType;
        }

        public override bool Resolve(Target target, GameObject source, object targetObject)
        {
            try
            {
                if (target.IsCollection)
                    ResolveCollection(source, targetObject, target);
                else
                    ResolveSingle(source, targetObject, target);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }

            return false;
        }

        protected abstract void ResolveSingle(GameObject source, object targetObject, Target target);
        protected abstract void ResolveCollection(GameObject source, object targetObject, Target target);

        void IPostBuildTargetChecker.CheckTarget(Target target)
        {
            if (target.MemberType != typeof(UnityEngine.Object) && !target.MemberType.IsSubclassOf(typeof(UnityEngine.Object)))
                throw new ArgumentException("Target should be UnityEngine.Object or derived from it");

            if (exactType == null)
                return;
            
            if (!target.MemberType.IsAssignableFrom(exactType))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Can't assign \"");
                builder.Append(ExactType.Name);
                builder.Append("\" to \"");
                builder.Append(target.Member.Name);
                builder.Append("\" of type \"");
                builder.Append(target.MemberType.Name);
                builder.Append("\" declared in class \"");
                builder.Append(target.Member.DeclaringType.Name);
                builder.Append("\"");

                throw new ArgumentException(builder.ToString());
            }
        }
    }
}