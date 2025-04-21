using System.IO;
using System.Linq;
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
        
        [MenuItem("OSK-Framework/Install Dependencies/DevConsole")]
        public static void InstallDependenciesDevConsole()
        {
            AddPackage("https://github.com/O-S-K/DevConsole.git");
        }

        [MenuItem("OSK-Framework/Install Dependencies/Observable")]
        public static void InstallDependenciesObservable()
        {
            AddPackage("https://github.com/O-S-K/OSK-Observable");
        } 
        

        private static void AddPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add(packageName);
            UnityEditor.EditorUtility.DisplayDialog("OSK-Framework", "Package added successfully", "OK");
            UnityEditor.AssetDatabase.Refresh();
        }

        private static void UpdatePackage(string packageName)
        {
            string path = System.IO.Path.Combine(Application.dataPath, "Packages", packageName);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                UnityEditor.PackageManager.Client.Add(packageName);
                UnityEditor.EditorUtility.DisplayDialog("OSK-Framework", "Package updated successfully", "OK");
                UnityEditor.AssetDatabase.Refresh();
            }
            else
            {
                UnityEditor.EditorUtility.DisplayDialog("OSK-Framework", "Package not found", "OK");
            }
        }
    }
}