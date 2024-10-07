using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using OSK;
using UnityEngine;

public class NamespaceTool : EditorWindow
{
    private string targetNamespace = "YourNamespaceHere"; // Namespace mục tiêu
    private string folderPath = "Assets/Scripts"; // Đường dẫn tới thư mục chứa script
    private string indent = "    "; // 4 dấu cách để thụt vào

    [MenuItem("Tools/Namespace Tool")]
    public static void ShowWindow()
    {
        GetWindow<NamespaceTool>("Namespace Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Namespace Tool", EditorStyles.boldLabel);
        targetNamespace = EditorGUILayout.TextField("Namespace:", targetNamespace);
        folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);

        if (GUILayout.Button("Apply Namespace"))
        {
            ApplyNamespace();
        }
    }

    private void ApplyNamespace()
    {
        string[] scriptFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        foreach (string scriptFile in scriptFiles)
        {
            string scriptContent = File.ReadAllText(scriptFile);

            // Kiểm tra xem script có namespace chưa
            if (Regex.IsMatch(scriptContent, @"namespace\s+\w+"))
            {
                // Nếu có namespace, thay thế bằng namespace mới
                scriptContent = Regex.Replace(scriptContent, @"namespace\s+\w+", $"namespace {targetNamespace}");
            }
            else
            {
                // Nếu chưa có namespace, thêm vào
                scriptContent = AddNamespaceToScript(scriptContent);
            }

            File.WriteAllText(scriptFile, scriptContent);
            Logg.Log("Namespace applied to: " + scriptFile);
        }
        AssetDatabase.Refresh();
    }

    private string AddNamespaceToScript(string scriptContent)
    {
        // Tìm vị trí bắt đầu của class, struct, enum, hoặc interface và bỏ qua các attribute trên đầu
        string pattern = @"\n*(\[.*?\])*\s*(public|internal|private|protected)?\s*(class|struct|enum|interface)\s+\w+";
        Match match = Regex.Match(scriptContent, pattern);

        if (match.Success)
        {
            int index = match.Index;
            string beforeClass = scriptContent.Substring(0, index);
            string afterClass = scriptContent.Substring(index);

            // Thêm namespace bao quanh phần sau các thuộc tính và trước class
            string indentedContent = IndentContent(afterClass);
            return $"{beforeClass}\nnamespace {targetNamespace}\n{{\n{indentedContent}\n}}";
        }

        return scriptContent;
    }

    private string IndentContent(string content)
    {
        // Thụt lề cho mỗi dòng của nội dung bên trong namespace
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            // Thêm khoảng trắng thụt lề nếu dòng không rỗng
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                lines[i] = indent + lines[i];
            }
        }

        return string.Join("\n", lines);
    }
}
