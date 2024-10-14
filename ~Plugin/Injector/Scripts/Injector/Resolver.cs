using System;
using UnityEngine;

namespace Injector
{
    public static class Resolver
    {
        public static void Resolve(GameObject resolveGameObject, object resolveTarget, bool rethrowExceptions = false)
        {
            if (resolveGameObject == null)
                throw new ArgumentException("Resolve gameObject should not be null");

            if (resolveTarget == null)
                throw new ArgumentException("Resolve target should not be null");

            var type = resolveTarget.GetType();
            var descriptor = DescriptorsHolder.GetDescriptor(type, rethrowExceptions);
            for (int index = 0; index < descriptor.TargetsCount; index++)
            {
                var target = descriptor[index];
                for (int attributeIndex = 0; attributeIndex < target.AttributesCount; attributeIndex++)
                {
                    try
                    {
                        var attribute = target[attributeIndex];
                        if (attribute is GameObjectInjectorAttribute)
                            (attribute as GameObjectInjectorAttribute).Resolve(target, resolveGameObject, resolveTarget);
                        else if (attribute is ObjectIndependentInjectorAttribute)
                            (attribute as ObjectIndependentInjectorAttribute).Resolve(target, resolveTarget);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Exception during resolving " + resolveTarget.GetType().Name + " with " + resolveGameObject.name + " game object: " + ex.Message);
                    }
                }
            }
        }
    }
}