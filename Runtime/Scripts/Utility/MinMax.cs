using UnityEngine;

namespace OSK
{

    [System.Serializable]
    public class MinMaxFloat
    {
        public float min;
        public float max;

        public float RandomValue => Random.Range(min, max);
        public float TimeAverage => (min + max) / 2;

        public MinMaxFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public MinMaxFloat(float min, float max, bool randomize)
        {
            this.min = min;
            this.max = max;

            if (randomize)
            {
                this.min = Random.Range(min, max);
                this.max = Random.Range(min, max);
            }
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
