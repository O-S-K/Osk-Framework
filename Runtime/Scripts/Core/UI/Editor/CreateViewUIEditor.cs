#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;


namespace OSK
{
    public class CreateViewUIEditor : EditorWindow
    {
        private string scriptName = "NewView";

        [MenuItem("OSK-Framework/UI/Create view")]
        public static void ShowWindow()
        {
            GetWindow<CreateViewUIEditor>("Create View");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create New View", EditorStyles.boldLabel);

            // Input field for script name
            scriptName = EditorGUILayout.TextField("Script Name", scriptName);

            if (GUILayout.Button("Create View"))
            {
                CreateAndAttachView(scriptName);
            }
        }

        private static void CreateAndAttachView(string scriptName)
        {
            if (string.IsNullOrEmpty(scriptName))
            {
                EditorUtility.DisplayDialog("Error", "Script name cannot be empty.", "OK");
                return;
            }
            
            // create forlder
            string folderPath = "Assets/ViewsCreator";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Views");
            }

            // Create the script if it doesn't exist
            string scriptPath = $"Assets/ViewsCreator/{scriptName}.cs";
            if (!File.Exists(scriptPath))
            {
                CreateScriptFile(scriptPath, scriptName);
                AssetDatabase.Refresh();
            }

            // Đợi biên dịch hoàn tất
            EditorApplication.delayCall += () => WaitForCompilationAndAttach(scriptPath, scriptName);
        }

        private static void WaitForCompilationAndAttach(string scriptPath, string scriptName)
        {
            if (EditorApplication.isCompiling)
            {
                Debug.Log("Waiting for Unity to finish compiling...");
                EditorApplication.delayCall += () => WaitForCompilationAndAttach(scriptPath, scriptName);
                return;
            }

            // Tìm MonoScript
            MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            if (monoScript == null)
            {
                Debug.LogError($"Failed to load MonoScript at '{scriptPath}'.");
                return;
            }

            System.Type scriptType = monoScript.GetClass();
            if (scriptType == null)
            {
                Debug.LogError($"Failed to get class type for script '{scriptName}'. Check for compilation errors.");
                return;
            }

            // Tạo GameObject và gắn script
            AttachScriptToGameObject(scriptName, scriptType);
        }
        
        private static void AttachScriptToGameObject(string scriptName, System.Type scriptType)
        {
            var viewContainer = FindObjectOfType<ViewContainer>();
            GameObject view = new GameObject(scriptName);

            // Thêm các component cần thiết
            view.GetOrAdd<RectTransform>();
            view.GetOrAdd<CanvasRenderer>();
            view.AddComponent(scriptType); 

            // Đặt làm con của ViewContainer nếu có
            if (viewContainer != null)
                view.transform.SetParent(viewContainer.transform);

            // Cài đặt vị trí và kích thước
            var rectTransform = view.transform.Get<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Tạo container con
            var container = new GameObject("Container");
            container.transform.SetParent(view.transform);
            container.transform.localPosition = Vector3.zero;
            container.transform.localScale = Vector3.one;

            var containerRect = container.GetOrAdd<RectTransform>();
            containerRect.sizeDelta = Vector2.zero;
            containerRect.anchorMin = Vector2.zero;
            containerRect.anchorMax = Vector2.one;
            containerRect.pivot = new Vector2(0.5f, 0.5f);

            // Chọn GameObject trong Hierarchy
            Selection.activeGameObject = view;

            Debug.Log($"Created GameObject '{scriptName}' with script '{scriptName}' attached.");
        }

        private static void CreateScriptFile(string path, string scriptName)
        {
            string scriptTemplate = $@"
    using UnityEngine;
    using OSK;

    public class {scriptName} : View
    {{
        public override void Initialize(ViewContainer viewContainer)
        {{
            base.Initialize(viewContainer);
        }} 

        public override void Open(object data = null)
        {{
            base.Open(data);
        }}

        public override void Hide()
        {{
            base.Hide();
        }}
    }}";

            File.WriteAllText(path, scriptTemplate);
            Debug.Log($"Script '{scriptName}' created at '{path}'.");
        }
    }
}
#endif