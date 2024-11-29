using UnityEngine;

namespace OSK
{

    [System.Serializable]
    public class MinMaxFloat
    {
        public float min;
        public float max;

        public float RandomValue => Random.Range(min, max);

        public MinMaxFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, min, max);
        }
    }

    [System.Serializable]
    public class MinMaxInt
    {
        public int min;
        public int max;

        public int RandomValue => Random.Range(min, max);

        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public float Clamp(int value)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
}
