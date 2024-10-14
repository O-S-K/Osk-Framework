using System;
using System.Collections.Generic;

namespace Injector
{
    public static class DescriptorsHolder
    {
        private static Dictionary<Type, TargetsHolder> descriptors = new Dictionary<Type, TargetsHolder>();

        public static void ClearCache()
        {
            descriptors = new Dictionary<Type, TargetsHolder>();
        }

        public static TargetsHolder GetDescriptor(Type type, bool rethrowException = false)
        {
            TargetsHolder descriptor = null;
            if (!descriptors.TryGetValue(type, out descriptor))
            {
                descriptor = TargetsHolder.BuildDescriptor(type, rethrowException);
                descriptors.Add(type, descriptor);
            }

            return descriptor;
        }
    }
}