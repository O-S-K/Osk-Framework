using System.Linq;
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
                PrefabUtility.InstantiatePrefab(Resources.LoadAll<Main>("").First());
            }
        }

        [MenuItem("OSK-Framework/Create UIRoot")]
        public static void CreateHUDOnScene()
        {
            if (FindObjectOfType<RootUI>() == null)
            {
                PrefabUtility.InstantiatePrefab(Resources.LoadAll<RootUI>("").First());
            }
        }

        [MenuItem("OSK-Framework/Install Dependencies/Dotween")]
        public static void InstallDependenciesDotween()
        {
            AddPackage("https://github.com/O-S-K/DOTween.git");
        }

        [MenuItem("OSK-Framework/Install Dependencies/UIFeel")]
        public static void InstallDependenciesUIFeel()
        {
            AddPackage("https://github.com/O-S-K/UIFeel.git");
        }

        private static void AddPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add(packageName);
        }
    }
}