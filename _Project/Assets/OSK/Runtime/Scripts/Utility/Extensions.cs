using UnityEngine;
using UnityEngine.UI;
using TMPro;
public static class Extensions
{
    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        return component != null ? component : gameObject.AddComponent<T>();
    }
    
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        float v = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        return v;
    }
    
    public static float WrapAngle(this float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    public static T ChangeAlpha<T>(this T g, float newAlpha) where T : Graphic
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }

    public static void SetTextFade(this Text text, float value)
    {
        Color color = text.color;
        color.a = value;
        text.color = color;
    }
    /// <summary>
    /// SetAlpha: 0 -> 1
    /// </summary>
    /// <param name="text"></param>
    /// <param name="value"></param>
    public static void SetTextFade(this TextMeshProUGUI text, float value)
    {
        Color color = text.color;
        color.a = value;
        text.color = color;
    }

    public static void AnulateRotationExceptY(this Transform transform)
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;
    }

    /// <summary>
    /// SetAlpha: 0 -> 1
    /// </summary>
    /// <param name="color"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    public static void SetAlpha(UnityEngine.UI.Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }


    public static void SetColorMaterial(this Material material, string nameID, Color color)
    {
        var nameId = Shader.PropertyToID(nameID);
        material.SetColor(nameId, color);
    }

    public static void SetColorMaterials(this Material[] materials, string nameID, Color color)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor(Shader.PropertyToID(nameID), color);
        }
    }

    // LAYER
    public static void SetLayerAllChildren(this GameObject gameobject, string nameLayer)
    {
        foreach (GameObject child in gameobject.GetComponentsInChildren<GameObject>())
        {
            child.layer = LayerMask.NameToLayer(nameLayer);
        }
    }

    public static void SetLayer(this GameObject gameObject, int layer, bool applyToChildren = false)
    {
        gameObject.layer = layer;
        if (applyToChildren)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetLayer(gameObject.transform.GetChild(i).gameObject, layer, true);
            }
        }
    }

    public static void SetLayer(this GameObject gameObject, string nameLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(nameLayer);
    }


    public static void DestroyAllChildren(this Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }
    }
    
    // example : transform.position =  transform.position.With(y: 5);
    public static Vector3 With(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector3.x, y ?? vector3.y, z ?? vector3.z);
    }     
    
    public static Vector3 WithX(this Vector3 vector3, float x)
    {
        return new Vector3(x, vector3.y, vector3.z);
    }
    
    public static Vector3 WithY(this Vector3 vector3, float y)
    {
        return new Vector3(vector3.x, y, vector3.z);
    }
    
    public static Vector3 WithZ(this Vector3 vector3, float z)
    {
        return new Vector3(vector3.x, vector3.y, z);
    }
    
    // example : transform.position =  transform.position.Add(x: 1, y: 5);
    public static Vector3 Add(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(vector3.x + (x ?? 0), vector3.y + (y ?? 0), vector3.z + (z ?? 0));
    }
    
    public static Color WithA(this Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }
    
    
	public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
	{
		return Vector3.Normalize(destination - source);
	}
    
    
    public static int ConvertExcelToInt(string input)
    {
        string cleanedInput = input.Replace(" ", "");
        if (int.TryParse(cleanedInput, out int result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning($"Unable to convert '{cleanedInput}' to an integer.");
            return -1; // Default value if conversion fails
        }
    }
}
