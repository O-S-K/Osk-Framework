using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ScriptGeneratorTool : EditorWindow
{
    private List<string> scriptNames = new List<string>();
    private List<TypeScript> typeScripts = new List<TypeScript>();
    
    private string folderPath = "Assets/Scripts/"; 
    private int scriptCount = 1;
    
    private TypeScript typeScript;
    public enum TypeScript
    {
        MonoBehaviour,
        UIComponent,
        StateMachine,
        Singleton,
        Interface,
        Enum,
        Struct 
    }
    

    [MenuItem("OSK-Framework/Tools/Script Generator Tool")]
    public static void ShowWindow()
    {
        GetWindow<ScriptGeneratorTool>("Script Generator Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Generator", EditorStyles.boldLabel);

        // Nhập đường dẫn lưu script
        folderPath = EditorGUILayout.TextField("Enter Folder Path", folderPath);
        scriptCount = EditorGUILayout.IntField("Number of Scripts", scriptCount);
        EditorGUILayout.Space(10);

        // Đảm bảo danh sách scriptNames có đúng số lượng phần tử
        while (scriptNames.Count < scriptCount)
        {
            scriptNames.Add(""); // Thêm phần tử rỗng nếu số lượng script tăng
            typeScripts.Add(TypeScript.MonoBehaviour);
        }
        while (scriptNames.Count > scriptCount)
        {
            scriptNames.RemoveAt(scriptNames.Count - 1); // Xóa phần tử thừa nếu số lượng script giảm
            typeScripts.RemoveAt(typeScripts.Count - 1);
        }

        // Hiển thị từng trường nhập cho các tên script
        for (int i = 0; i < scriptCount; i++)
        {
            EditorGUILayout.Space();
            scriptNames[i] = EditorGUILayout.TextField($"Script - {i + 1} Name", scriptNames[i]);
            typeScripts[i] = (TypeScript)EditorGUILayout.EnumPopup($"Type - {i + 1}", typeScripts[i]);
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Generate Scripts"))
        {
            GenerateScripts();
        }
    }

    private void GenerateScripts()
    {
        // Kiểm tra và tạo thư mục nếu chưa tồn tại
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Thư mục '{folderPath}' đã được tạo.");
        }

        for (int i = 0; i < scriptNames.Count; i++)
        {
            if (!string.IsNullOrEmpty(scriptNames[i].Trim()))
            {
                CreateScript(scriptNames[i].Trim(), typeScripts[i]);
            }
        }
        AssetDatabase.Refresh(); // Cập nhật AssetDatabase để hiện thị các file script mới
    }

    private void CreateScript(string scriptName ,TypeScript typeScript)
    {
        // Đường dẫn tới file script
        string filePath = Path.Combine(folderPath, scriptName + ".cs");

        // Kiểm tra xem file đã tồn tại hay chưa
        if (File.Exists(filePath))
        {
            Debug.LogWarning($"Script {scriptName} đã tồn tại tại đường dẫn: {filePath}");
            return; // Nếu tồn tại thì không tạo lại
        }
        string scriptTemplate = string.Empty;

        switch (typeScript)
        {
            case TypeScript.MonoBehaviour:
                scriptTemplate = "using UnityEngine;\n\n" +
                                 "public class " + scriptName + " : MonoBehaviour\n" +
                                 "{\n" +
                                 "}\n";
                break;
            case TypeScript.UIComponent:
                scriptTemplate = "using UnityEngine;\n" +
                                 "using UnityEngine.UI;\n\n" +
                                 "public class " + scriptName + " : MonoBehaviour\n" +
                                 "{\n" +
                                 "}\n";
                break;
            case TypeScript.StateMachine:
                break;
            case TypeScript.Singleton:
                scriptTemplate = "using UnityEngine;\n" +
                                 "using OSK;\n\n" +
                                 "public class " + scriptName + " : SingletonMono<scriptName>\n" +
                                 "{\n" +
                                 "}\n";
                break;
            case TypeScript.Interface:
                scriptTemplate =  "public interface " + scriptName + " \n" +
                                 "{\n" +
                                 "}\n";
                break;
            case TypeScript.Enum:
                scriptTemplate =  "public enum " + scriptName + " \n" +
                                  "{\n" +
                                  "}\n";
                break;
            case TypeScript.Struct:
                scriptTemplate =  "public struct " + scriptName + " \n" +
                                  "{\n" +
                                  "}\n";
                break;
        }

        // Tạo file script mới
        File.WriteAllText(filePath, scriptTemplate);
        Debug.Log($"Script {scriptName} đã được tạo tại đường dẫn: {filePath}");
    }
}