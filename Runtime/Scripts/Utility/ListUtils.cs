using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public static class ListUtils
    { 
        public delegate TResult MapFunc<out TResult, TArg>(TArg arg);
        public delegate bool FilterFunc<TArg>(TArg arg);

        public static List<Vector3> AddFirstList(this List<Vector3> list, Vector3 obj)
        {
            list.Insert(0, obj);
            return list;
        }
        
        public static List<Vector3> AddLastList(this List<Vector3> list, Vector3 obj)
        {
            list.Add(obj);
            return list;
        }
        
        public static List<TOut> Map<TIn, TOut>(List<TIn> list, MapFunc<TOut, TIn> func)
        {
            List<TOut> newList = new List<TOut>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                newList.Add(func(list[i]));
            }

            return newList;
        } 

        public static bool CompareLists<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = list1.Count - 1; i >= 0; i--)
            {
                bool found = false;

                for (int j = 0; j < list2.Count; j++)
                {
                    if (list1[i].Equals(list2[j]))
                    {
                        found = true;
                        list1.RemoveAt(i);
                        list2.RemoveAt(j);
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

       public static T GetClosest<T>(this T[] collection, Vector3 toPoint) where T : Component
        {
            if (collection == null || collection.Length == 0)
                return null;

            T closest = null;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < collection.Length; i++)
            {
                float sqrd = (collection[i].transform.position - toPoint).sqrMagnitude;
                if (sqrd < closestDistance)
                {
                    closestDistance = sqrd;
                    closest = collection[i];
                }
            }

            return closest;
        }

        public static T GetClosest<T>(this IReadOnlyList<T> collection, Vector3 toPoint) where T : Component
        {
            if (collection == null || collection.Count == 0)
                return null;

            T closest = null;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < collection.Count; i++)
            {
                float sqrd = (collection[i].transform.position - toPoint).sqrMagnitude;
                if (sqrd < closestDistance)
                {
                    closestDistance = sqrd;
                    closest = collection[i];
                }
            }
            return closest;
        }
        
        

        public static void RefreshList<T>(this List<T> list) where T : class
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                    list.RemoveAt(i);
            }
        }
    }
}
