using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public static class RandomUtils 
    {
        private static System.Random _random;

        static RandomUtils()
        {
            _random = new System.Random();
        }

        public static T GetRandom<T>(params T[] arr)
        {
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }

        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }

        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max + .0001f);
        }

        public static Quaternion RandomRotation()
        {
            return UnityEngine.Random.rotationUniform;
        }

        public static Color RandomColor()
        {
            return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }

        public static Color RandomColorHSV()
        {
            return UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f);
        }

        public static Vector2 RandomUnitCircle(float radius)
        {
            return UnityEngine.Random.insideUnitSphere * radius;
        }

        public static Vector3 RandomUnitSphere(float radius)
        {
            return UnityEngine.Random.onUnitSphere * radius;
        }
        
        public static Vector3 RandomVector3(Vector3 min, Vector3 max)
        {
            return  new Vector3(RandomFloat(min.x, max.x), RandomFloat(min.y, max.y), RandomFloat(min.z, max.z));
        }

        public static int UniqueRandomInt(int min, int max)
        {
            List<int> usedValues = new List<int>();
            int val = UnityEngine.Random.Range(min, max);
            while (usedValues.Contains(val))
            {
                val = UnityEngine.Random.Range(min, max);
            }
            return val;
        }

        public static void InitSeed(int seed)
        {
            _random = new System.Random(seed);
        }

        public static int RandomSystem(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public static float RandomSystem(float min, float max)
        {
            return (float)_random.NextDouble() * (max + .0001f - min) + min;
        }

        public static bool RandomBool
        {
            get => UnityEngine.Random.value > 0.5f;
        }

        public static int RandomSign
        {
            get => RandomBool ? 1 : -1;
        }

        public static void RandomRange<T>(List<T> rangeToRandom, int startIndex = 0, int endIndex = -1)
        {
            if (rangeToRandom == null) return;
            if (rangeToRandom.Count == 0) return;
            if (endIndex < startIndex || endIndex >= rangeToRandom.Count)
            {
                endIndex = rangeToRandom.Count - 1;
            }
            for (int i = startIndex; i < endIndex; i++)
            {
                int rnd = UnityEngine.Random.Range(startIndex, endIndex + 1);
                (rangeToRandom[rnd], rangeToRandom[i]) = (rangeToRandom[i], rangeToRandom[rnd]);
            }
        }

        public static float GetRandomNumber(float minimum, float maximum)
        {
            var random = new System.Random();
            return (float)random.NextDouble() * (maximum - minimum) + minimum;
        }
 
        public static void Shuffle<T>(List<T> list)
        {
            var rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        //public static  T RandomItemDropInPercent<T>(List<T> listItem)
        //{
        //    int randomNumber = RandomInt(1, 101);
        //    List<T> possibleItems = new List<T>();
        //    foreach (T item in listItem)
        //    {
        //        if (randomNumber <= item.percent)
        //        {
        //            possibleItems.Add(item);
        //        }
        //    }
        //    if (possibleItems.Count > 0)
        //    {
        //        T droppedItem = possibleItems[RandomInt(0, possibleItems.Count)];
        //        return droppedItem;
        //    }
        //    Debug.Log("Nothing was dropped");
        //    return default;
        //}
    }
}
