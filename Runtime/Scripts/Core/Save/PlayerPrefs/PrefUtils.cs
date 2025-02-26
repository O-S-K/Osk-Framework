using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OSK
{
    // https://github.com/herbou/UnityPlayerPrefsExtra
    // Vectors, Colors, Quaternions, Lists, and Your Pre defined types (Object) [classes or structs].
    public class PrefUtils
    {
        #region Int -----------------------------------------------------------------------------------------

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        #endregion

        #region Float -----------------------------------------------------------------------------------------

        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key, 0f);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        #endregion

        #region String -----------------------------------------------------------------------------------------

        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(key, "");
        }

        public static string GetString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        #endregion

        #region Bool -----------------------------------------------------------------------------------------

        public static bool GetBool(string key)
        {
            return (PlayerPrefs.GetInt(key, 0) == 1);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            return (PlayerPrefs.GetInt(key, (defaultValue ? 1 : 0)) == 1);
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, (value ? 1 : 0));
        }

        #endregion

        #region Vector 2 -----------------------------------------------------------------------------------------

        public static Vector2 GetVector2(string key)
        {
            return Get<Vector2>(key, Vector2.zero);
        }

        public static Vector2 GetVector2(string key, Vector2 defaultValue)
        {
            return Get<Vector2>(key, defaultValue);
        }

        public static void SetVector2(string key, Vector2 value)
        {
            Set(key, value);
        }

        #endregion

        #region Vector 3 -----------------------------------------------------------------------------------------

        public static Vector3 GetVector3(string key)
        {
            return Get<Vector3>(key, Vector3.zero);
        }

        public static Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return Get<Vector3>(key, defaultValue);
        }

        public static void SetVector3(string key, Vector3 value)
        {
            Set(key, value);
        }

        #endregion

        #region Vector 4 -----------------------------------------------------------------------------------------

        public static Vector4 GetVector4(string key)
        {
            return Get<Vector4>(key, Vector4.zero);
        }

        public static Vector4 GetVector4(string key, Vector4 defaultValue)
        {
            return Get<Vector4>(key, defaultValue);
        }

        public static void SetVector4(string key, Vector4 value)
        {
            Set(key, value);
        }

        #endregion

        #region Color -----------------------------------------------------------------------------------------

        public static Color GetColor(string key)
        {
            return Get<Color>(key, new Color(0f, 0f, 0f, 0f));
        }

        public static Color GetColor(string key, Color defaultValue)
        {
            return Get<Color>(key, defaultValue);
        }

        public static void SetColor(string key, Color value)
        {
            Set(key, value);
        }

        #endregion

        #region Quaternion -----------------------------------------------------------------------------------------

        public static Quaternion GetQuaternion(string key)
        {
            return Get<Quaternion>(key, Quaternion.identity);
        }

        public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
        {
            return Get<Quaternion>(key, defaultValue);
        }

        public static void SetQuaternion(string key, Quaternion value)
        {
            Set(key, value);
        }

        #endregion

        #region List -----------------------------------------------------------------------------------------

        public class ListWrapper<T>
        {
            public List<T> list = new List<T>();
        }

        public static List<T> GetList<T>(string key)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T>()).list;
        }

        public static List<T> GetList<T>(string key, List<T> defaultValue)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T> { list = defaultValue }).list;
        }

        public static Dictionary<string, T> GetDictionary<T>(string key)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T>()).list.ToDictionary(x => x.ToString());
        }

        public static void SetList<T>(string key, List<T> value)
        {
            Set(key, new ListWrapper<T> { list = value });
        }

        public static void SetDictionary<T>(string key, Dictionary<string, T> value)
        {
            Set(key, new ListWrapper<T> { list = value.Values.ToList() });
        }

        #endregion

        #region Object -----------------------------------------------------------------------------------------

        public static T GetObject<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        public static T GetObject<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue);
        }

        public static void SetObject<T>(string key, T value)
        {
            Set(key, value);
        }

        #endregion

        #region Enum -----------------------------------------------------------------------------------------

        public static T GetEnum<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        public static T GetEnum<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue);
        }

        public static void SetEnum<T>(string key, T value)
        {
            Set(key, value);
        }

        #endregion


        //Generic template ---------------------------------------------------------------------------------------
        private static T Get<T>(string key, T defaultValue)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key, JsonUtility.ToJson(defaultValue)));
        }

        private static void Set<T>(string key, T value)
        {
            if (value == null)
            {
                Debug.LogError("Value is null!");
                return;
            }

            string jsonString = JsonUtility.ToJson(value, true); // prettyPrint để dễ đọc
            Debug.Log("Pretty JSON String: " + jsonString);

            PlayerPrefs.SetString(key, jsonString);
            PlayerPrefs.Save();
        }

        private static T Get<T>(string key)
        {
            string jsonString = PlayerPrefs.GetString(key);
            Debug.Log("Pretty JSON String: " + jsonString);

            return JsonUtility.FromJson<T>(jsonString);
        }

        public static void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}