using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{
    public class FromStaticMethodAttribute : FromMethodBase
    {
        private Type type;

        public FromStaticMethodAttribute(string method) : base(method)
        {
        }

        public FromStaticMethodAttribute(Type type, string method) : base(method)
        {
            this.type = type;
        }

        protected override bool IsStatic
        {
            get
            {
                return true;
            }
        }

        protected override Type GetTypeToUse(Target target)
        {
            return type == null ? target.Member.DeclaringType : type;
        }
    }
}