using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public static class Utilities
    { 
        public delegate TResult MapFunc<out TResult, TArg>(TArg arg);
        public delegate bool FilterFunc<TArg>(TArg arg);

        public static List<TOut> Map<TIn, TOut>(List<TIn> list, MapFunc<TOut, TIn> func)
        {
            List<TOut> newList = new List<TOut>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                newList.Add(func(list[i]));
            }

            return newList;
        }

        public static char CharToLower(this char c)
        {
            return (c >= 'A' && c <= 'Z') ? (char)(c + ('a' - 'A')) : c;
        }

        public static string CalculateMD5Hash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            Debug.Log("CalculateMD5Hash" +  sb.ToString());
            return sb.ToString();
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

        public static void PrintList<T>(List<T> list)
        {
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                {
                    str += ", ";
                }
                str += list[i].ToString();
            }
            Debug.Log(str);
        }
    }
}
