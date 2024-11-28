using OSK;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    public class OSKEditor : MonoBehaviour
    {
        [MenuItem("OSK-Framework/Create Framework")]
        public static  void CreateWorldOnScene()
        { 
            if (FindObjectOfType<Main>() == null)
            {
                PrefabUtility.InstantiatePrefab(Resources.Load<Main>("OSK-Framework"));
                PrefabUtility.InstantiatePrefab(Resources.Load<RootUI>("HUD/HUD"));
                Debug.Log("OSK-Framework created".Bold().Color(Color.green));
            }
        }
    }
}