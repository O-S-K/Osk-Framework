using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Injector
{
    public abstract class FromMethodBase : ObjectIndependentInjectorAttribute, IPostBuildTargetChecker
    {
        private static MethodsCache cache = new MethodsCache();
        protected static MethodsCache Cache
        {
            get
            {
                return cache;
            }
        }

        private static ElementTypesHolder typesCache = new ElementTypesHolder();
        protected static ElementTypesHolder TypesCache
        {
            get
            {
                return typesCache;
            }
        }

        protected class MethodsHolder
        {
            bool staticOnly;
            Type type;
            Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

            public MethodsHolder(Type type, bool staticOnly)
            {
                this.type = type;
                this.staticOnly = staticOnly;
            }

            public MethodInfo GetMethodOrGetProperty(string methodName)
            {
                MethodInfo info = null;
                if (!methods.TryGetValue(methodName, out info))
                {
                    var binding = BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.NonPublic;
                    binding |= (staticOnly ? BindingFlags.Static : BindingFlags.Instance);

                    var methodsOfType = type.GetMethods(binding);

                    foreach (var method in methodsOfType)
                    {
                        if (method.Name == methodName && method.GetParameters().Length == 0)
                        {
                            info = method;
                            break;
                        }
                    }

                    if (info == null)
                    {
                        var properties = type.GetProperties(binding);
                        foreach (var property in properties)
                        {
                            if (property.Name == methodName)
                            {
                                info = property.GetGetMethod(true);
                                break;
                            }
                        }
                    }

                    methods.Add(methodName, info);
                }

                return info;
            }
        }

        protected class MethodsCache
        {
            Dictionary<Type, MethodsHolder> instanceHolders = new Dictionary<Type, MethodsHolder>();
            Dictionary<Type, MethodsHolder> staticHolders = new Dictionary<Type, MethodsHolder>();

            public MethodInfo GetMethod(Type type, string name, bool isStatic)
            {
                Dictionary<Type, MethodsHolder> dictionaryToUse = isStatic ? staticHolders : instanceHolders;

                MethodsHolder holder = null;
                if (!dictionaryToUse.TryGetValue(type, out holder))
                {
                    holder = new MethodsHolder(type, isStatic);
                    dictionaryToUse.Add(type, holder);
                }

                return holder.GetMethodOrGetProperty(name);
            }
        }

        protected class ElementTypesHolder
        {
            Dictionary<Type, Type> types = new Dictionary<Type, Type>();

            public Type GetElementTypeOf(Type type)
            {
                Type result = null;
                if (!types.TryGetValue(type, out result))
                {
                    if (type.IsArray)
                        result = type.GetElementType();
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                        result = type.GetGenericArguments()[0];

                    types.Add(type, result);
                }

                return result;
            }
        }

        protected string method;

        protected abstract bool IsStatic
        {
            get;
        }

        public FromMethodBase(string method)
        {
            this.method = method;
        }

        protected virtual Type GetTypeToUse(Target target)
        {
            return target.Member.DeclaringType;
        }

        public override void Resolve(Target target, object targetObject)
        {
            (this as IPostBuildTargetChecker).CheckTarget(target);

            var typeToUse = GetTypeToUse(target);
            var methodToUse = Cache.GetMethod(typeToUse, method, IsStatic);

            var returnType = methodToUse.ReturnType;

            if (target.IsCollection)
            {
                var elementType = TypesCache.GetElementTypeOf(returnType);

                try
                {
                    target.SetFromList(targetObject, methodToUse.Invoke(IsStatic ? null : targetObject, null) as IList);
                }
                catch (TargetInvocationException exception)
                { 
                    throw new Exception("Error while calling user method \"" + method + "\" " + exception.InnerException.Message + "\n" + exception.InnerException.StackTrace);
                }
            }
            else
            {
                try
                {
                    target.Set(targetObject, methodToUse.Invoke(IsStatic ? null : targetObject, null));
                }
                catch (TargetInvocationException exception)
                {
                    throw new Exception("Error while calling user method \"" + method + "\" " + exception.InnerException.Message + "\n" + exception.InnerException.StackTrace);
                }
            }
        }

        void IPostBuildTargetChecker.CheckTarget(Target target)
        {
            var typeToUse = GetTypeToUse(target);
            var methodToUse = Cache.GetMethod(typeToUse, method, IsStatic);
            if (methodToUse == null)
                throw new ArgumentException("There is no " + (IsStatic ? "static method" : "method") + " \"" + method + "\" in \"" + typeToUse.Name + "\" which is required to get \"" + target.Member.Name + "\" field declared in \"" + target.Member.DeclaringType.Name + "\"");

            var returnType = methodToUse.ReturnType;

            if (target.IsCollection)
            {
                var elementType = TypesCache.GetElementTypeOf(returnType);
                if (elementType == null)
                    throw new ArgumentException("Incorrect return type of method \"" + method + "\" \"" + returnType.Name + "\" which can't be used to resolve dependency. Use List<> or array");

                if (!target.MemberType.IsAssignableFrom(elementType))
                    throw new ArgumentException("\"" + target.Member.Name + "\" of type \"" + target.Member.DeclaringType.Name + "\" can't be assigned from \"" + elementType.Name + "\" which is element of collection that returns \"" + method + "\" method declared in \"" + typeToUse.Name + "\"");
            }
            else
            {
                if (!target.MemberType.IsAssignableFrom(returnType))
                    throw new ArgumentException("Can't assign \"" + target.Member.Name + "\" of type \"" + target.MemberType.Name + "\" from method \"" + method + "\" which is declared in \"" + methodToUse.DeclaringType.Name + "\" and returns \"" + returnType.Name + "\"");
            }
        }
    }
}