using UnityEngine;

[System.Serializable]
public class MinMax
{
    public float min;
    public float max;

    public float RandomValue => Random.Range(min, max);

    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, min, max);
    }
}