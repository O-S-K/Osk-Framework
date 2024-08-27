using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class SingletonData<T> where T : class, new()
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
