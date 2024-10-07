using OSK;
using UnityEditor;
using UnityEngine;


namespace OSK
{

    [ExecuteAlways]
    public class OSKEditor : MonoBehaviour
    {
        [MenuItem("OSK-FrameWorld/Create Framework")]
        public static  void CreateWorldOnScene()
        {
            if (FindObjectOfType<Main>() == null)
            {
                PrefabUtility.InstantiatePrefab(Resources.Load<Main>("OSK-Framework"));
                PrefabUtility.InstantiatePrefab(Resources.Load<HUD>("HUD/HUD"));
                Debug.Log("OSK-Framework created".Bold().Color(ColorCustom.Green));
            }
        }
    }
}