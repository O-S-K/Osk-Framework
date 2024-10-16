using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class ScriptableObjectFinder : EditorWindow
{
    private Vector2 scrollPos;
    private string path = "Assets/Resources";  // Đặt mặc định là thư mục Resources
    private string[] scriptableObjectPaths;

    [MenuItem("OSK-Framework/Tools/Find All SO In Resources")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectFinder>("ScriptableObject Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("List of ScriptableObjects in Resources", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Path to search:", EditorStyles.label);
        path = GUILayout.TextField(path);  // Trường nhập đường dẫn
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Find ScriptableObjects"))
        {
            FindScriptableObjects();
        }

        if (GUILayout.Button("Clear Results"))
        {
            ClearResults();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        if (scriptableObjectPaths != null && scriptableObjectPaths.Length > 0)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - 100));

            foreach (var path in scriptableObjectPaths)
            {
                if (GUILayout.Button(Path.GetFileNameWithoutExtension(path), GUILayout.Height(20)))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    EditorGUIUtility.PingObject(asset);
                }
            }

            GUILayout.EndScrollView();
        }
    }

    private void FindScriptableObjects()
    {
        // Kiểm tra xem đường dẫn có tồn tại hay không
        if (!Directory.Exists(path))
        {
            Debug.LogError("Invalid path: " + path);
            return;
        }

        // Tìm tất cả các ScriptableObject trong thư mục đã chọn
        scriptableObjectPaths = Directory.GetFiles(path, "*.asset", SearchOption.AllDirectories)
            .Where(filePath => AssetDatabase.LoadAssetAtPath<ScriptableObject>(filePath) != null)
            .ToArray();
    }

    private void ClearResults()
    {
        // Xóa danh sách kết quả
        scriptableObjectPaths = null;
    }
}