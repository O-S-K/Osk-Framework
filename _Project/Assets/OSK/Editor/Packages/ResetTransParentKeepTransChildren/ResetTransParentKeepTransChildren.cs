#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

public class SaveTransform
{
    public Vector3[] position;
    public Quaternion[] rotation;
    public Vector3[] localScale;
}
 

[ExecuteInEditMode]
public class ResetTransParentKeepTransChildren : MonoBehaviour
{
    public static SaveTransform saveTransform = new SaveTransform();
    static Vector3 ScaleParent;
    static Transform objSelect;

    //[MenuItem("Edit/Reset Child &t", false, -101)]
    [MenuItem("Tools Editor/Reset Parent Keep Children &t")] 
    public static void FixTransforms()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.Log("Select the object before pressing tools");
            return;
        }
        objSelect = Selection.gameObjects[0].transform;

        ScaleParent = objSelect.localScale;
        SaveTransChilren();
        ResetParent();
        FixTransformChild();
    }

    public static void SaveTransChilren()
    {
        if (objSelect.childCount <= 0)
        {
            Debug.Log("Child Count <= 0");
            return;
        }
        //saveTransform.position = new Vector3[objSelect.childCount];
        //saveTransform.rotation = new Quaternion[objSelect.childCount];
        saveTransform.localScale = new Vector3[objSelect.childCount];

        for (int i = 0; i < objSelect.childCount; i++)
        {
            //saveTransform.position[i] = objSelect.GetChild(i).position;
            //saveTransform.rotation[i] = objSelect.GetChild(i).rotation;
            saveTransform.localScale[i] = objSelect.GetChild(i).localScale;
        }
    }

    public static void ResetParent()
    {
        objSelect.position = Vector3.zero;
        objSelect.rotation = Quaternion.identity;
        objSelect.localScale = Vector3.one;
    }

    public static void FixTransformChild()
    {
        if (objSelect.childCount <= 0)
        {
            Debug.Log("Child Count <= 0");
            return;
        }

        for (int i = 0; i < objSelect.childCount; i++)
        {
            //objSelect.GetChild(i).position = saveTransform.position[i];
            //objSelect.GetChild(i).rotation = saveTransform.rotation[i];

            var scaleParent = saveTransform.localScale[i];
            objSelect.GetChild(i).localScale = new Vector3(
                scaleParent.x * ScaleParent.x, 
                scaleParent.y * ScaleParent.y, 
                scaleParent.z * ScaleParent.z);
        }
    }
}
#endif