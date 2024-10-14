using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Injector
{
    public class Target
    {
        static object[] singleParameterArray = new object[] { null };

        private Target()
        {

        }

        Type memberExactType = null;
        bool isCollection = false;
        Type[] genericArguments = null;

        MethodInfo setMethod;

        MemberInfo member;
        public MemberInfo Member
        {
            get
            {
                return member;
            }
        }

        List<InjectorAttribute> attributes;
        Type[] interfaces;

        Type memberType;
        public Type MemberType
        {
            get
            {
                if (memberType != null)
                    return memberType;

                if (isCollection && memberExactType.IsArray)
                    memberType = memberExactType.GetElementType();
                else if (isCollection && genericArguments != null && genericArguments.Length > 0)
                    memberType = genericArguments[0];
                else
                    memberType = memberExactType;

                return memberType;
            }
        }
        
        public Type ExactMemberType
        {
            get
            {
                return memberExactType;
            }
        }

        public bool IsCollection
        {
            get
            {
                return isCollection;
            }
        }

        public int AttributesCount
        {
            get
            {
                return attributes.Count;
            }
        }

        public InjectorAttribute this[int index]
        {
            get
            {
                if (index < 0 || index >= attributes.Count)
                    throw new System.ArgumentOutOfRangeException("Index of attribute is out of range!");

                return attributes[index];
            }
        }

        public void Set(object owner, object value)
        {
            var member = Member;
            if (member.MemberType == MemberTypes.Field)
            {
                var fieldInfo = member as FieldInfo;
                fieldInfo.SetValue(owner, value);
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                singleParameterArray[0] = value;
                setMethod.Invoke(owner, parameters: singleParameterArray);
            }
            else
                throw new Exception("Unimplemented set behaviour \"" + member.MemberType.ToString() + "\"");
        }

        public void SetFromList(object owner, IList list, bool canBeSetDirectly = false)
        {
            if (!isCollection)
                throw new ArgumentException("Can't set IList to non collection target.");

            if (canBeSetDirectly && ExactMemberType.IsAssignableFrom(list.GetType()))
            {
                Set(owner, list);
                return;
            }

            var memberType = MemberType;
            var exactType = ExactMemberType;
            IList listToBeSet = null;

            if (exactType.IsArray)
            {
                listToBeSet = Get(owner) as IList;
                if (listToBeSet == null || listToBeSet.Count < list.Count)
                    listToBeSet = Array.CreateInstance(memberType, list.Count);

                int count = Mathf.Min(listToBeSet.Count, list.Count);
                for (int index = 0; index < count; index++)
                    listToBeSet[index] = list[index];

                int maxCount = Mathf.Min(listToBeSet.Count, list.Count);
                for (int index = count; index < maxCount; index++)
                    listToBeSet[index] = null;
            }
            else if (exactType.GetGenericTypeDefinition() == typeof(List<>))
            {
                listToBeSet = Get(owner) as IList;
                if (listToBeSet == null)
                    listToBeSet = Activator.CreateInstance(exactType) as IList;
                else
                    listToBeSet.Clear();

                for (int index = 0; index < list.Count; index++)
                    listToBeSet.Add(list[index]);
            }
            else
                throw new Exception("Unknown behaviour for \"" + MemberType.Name + "\"");

            Set(owner, listToBeSet);
        }

        public object Get(object owner)
        {
            if (Member.MemberType == MemberTypes.Field)
            {
                var fieldInfo = Member as FieldInfo;
                return fieldInfo.GetValue(owner);
            }
            else if (Member.MemberType == MemberTypes.Property)
            {
                var propertyType = Member as PropertyInfo;
                return propertyType.GetValue(owner, null);
            }
            else
                throw new Exception("Unimplemented get behaviour \"" + member.MemberType.ToString() + "\"");
        }

        private static Type GetExactType(MemberInfo info)
        {
            Type typeToReturn = null;
            if (info.MemberType == MemberTypes.Field)
            {
                var fieldInfo = info as FieldInfo;
                typeToReturn = fieldInfo.FieldType;
            }
            else if (info.MemberType == MemberTypes.Property)
            {
                var propertyType = info as PropertyInfo;
                typeToReturn = propertyType.PropertyType;
            }

            return typeToReturn;
        }

        public T TryGetAttribute<T>() where T : InjectorAttribute
        {
            foreach (var attribute in attributes)
                if (attribute is T)
                    return attribute as T;

            return null;
        }

        public static Target TryBuildTarget(MemberInfo member)
        {
            if (member.MemberType != MemberTypes.Field &&
                member.MemberType != MemberTypes.Property)
                return null;

            var attributes = member.GetCustomAttributes(typeof(InjectorAttribute), true);
            if (attributes == null || attributes.Length == 0)
                return null;

            List<InjectorAttribute> attributesToBeSet = new List<InjectorAttribute>();
            foreach (var attribute in attributes)
            {
                if (attribute is InjectorAttribute)
                    attributesToBeSet.Add(attribute as InjectorAttribute);
            }

            if (attributesToBeSet.Count == 0)
                return null;

            MethodInfo setMethod = null;
            if (member.MemberType == MemberTypes.Property)
            {
                var propertyInfo = member as PropertyInfo;
                if (!propertyInfo.CanWrite)
                    throw new ArgumentException("There are no \"set\" method for property \"" + member.Name + "\" of type \"" + member.DeclaringType.Name + "\"");

                setMethod = propertyInfo.GetSetMethod(true);
            }

            var target = new Target();
            target.member = member;
            target.memberExactType = GetExactType(member);
            target.setMethod = setMethod;
            target.attributes = attributesToBeSet;
            target.interfaces = target.memberExactType.GetInterfaces();
            target.isCollection = Array.Find(target.interfaces, x => x == typeof(ICollection)) != null;
            if (target.isCollection)
            {
                var exactType = target.memberExactType;
                if (!exactType.IsArray && exactType.GetGenericTypeDefinition() != typeof(List<>))
                    throw new ArgumentException("Injector only supports \"List<>\" and \"Array\"");
            }

            if (target.memberExactType.IsGenericType)
                target.genericArguments = target.memberExactType.GetGenericArguments();

            foreach (var attribute in attributesToBeSet)
            {
                var checker = attribute as IPostBuildTargetChecker;
                if (checker != null)
                    checker.CheckTarget(target);
            }

            return target;
        }
    }

    public sealed class TargetsHolder
    {
        List<Target> targets = new List<Target>();

        public int TargetsCount
        {
            get
            {
                return targets.Count;
            }
        }

        public Target this[int index]
        {
            get
            {
                if (index < 0 || index >= targets.Count)
                    throw new ArgumentOutOfRangeException("Index of target is out of range");

                return targets[index];
            }
        }

        private TargetsHolder()
        {

        }

        public static TargetsHolder BuildDescriptor(Type type, bool rethrowException = false)
        {
            TargetsHolder descriptor = new TargetsHolder();

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                try
                {
                    var target = Target.TryBuildTarget(field);
                    if (target == null)
                        continue;

                    descriptor.targets.Add(target);
                }
                catch (Exception ex)
                {
                    if (rethrowException)
                        throw ex;
                    else
                    {
                        StringBuilder builder = new StringBuilder();

                        builder.Append("Error while building target for field \"");
                        builder.Append(field.Name);
                        builder.Append("\" of type \"");
                        builder.Append(field.DeclaringType.Name);
                        builder.Append("\" with message: \"");
                        builder.Append(ex.Message);
                        builder.Append('"');

                        Debug.LogError(builder.ToString());
                    }
                }
            }

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var property in properties)
            {
                try
                {
                    var target = Target.TryBuildTarget(property);
                    if (target == null)
                        continue;

                    descriptor.targets.Add(target);
                }
                catch (Exception ex)
                {
                    if (rethrowException)
                        throw ex;
                    else
                    {
                        StringBuilder builder = new StringBuilder();

                        builder.Append("Error while building target for field \"");
                        builder.Append(property.Name);
                        builder.Append("\" of type \"");
                        builder.Append(property.DeclaringType.Name);
                        builder.Append("\" with message: \"");
                        builder.Append(ex.Message);
                        builder.Append('"');

                        Debug.LogError(builder.ToString());
                    }
                }
            }

            return descriptor;
        }
    }
}