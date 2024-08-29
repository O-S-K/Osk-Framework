using System;
using UnityEngine;

public static class Json
{
    public static T Read<T>(string json)
    {
        return JsonUtility.FromJson<JsonMapper<T>>(json).value;
    }

    public static string Write<T>(T obj)
    {
        return JsonUtility.ToJson(new JsonMapper<T>(obj));
    }

    [Serializable]
    public class JsonMapper<T>
    {
        public T value;

        public JsonMapper(T value)
        {
            this.value = value;
        }
    }
}