using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (instance == null)
                    {
                        instance = new GameObject().AddComponent<T>();
                        instance.gameObject.name = instance.GetType().Name;
                    }
                }
                return instance;
            }
        }
    }
}
