using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEditor;
using UnityEngine;

public class OSKEditor : MonoBehaviour
{
    [MenuItem("OSK-FrameWorld/Create Framework")]
    public static  void CreateWorldOnScene()
    {
        if (FindObjectOfType<Main>() == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load<Main>("OSK-Framework"));
            PrefabUtility.InstantiatePrefab(Resources.Load<HUD>("HUD/HUD"));

        }
    }
}
