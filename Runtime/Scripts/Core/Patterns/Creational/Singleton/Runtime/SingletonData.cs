using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    // SingletonData<T> can only be used with classes that have a default constructor (new()) and do not inherit MonoBehaviour
    // Example usage: SingletonDataInstance = SingletonData<MyClass>.Instance;
    // where MyClass is a class that has a default constructor. 
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
