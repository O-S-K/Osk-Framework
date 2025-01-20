#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace OSK
{
    public class CreateViewUIEditor : EditorWindow
    {
        private string scriptName = "NewView";
        private static bool isCreateShield = false;

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
            isCreateShield = EditorGUILayout.Toggle("Create Shield", isCreateShield);

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

            // create folder
            string folderPath = "Assets/ViewsCreator";
            IOUtility.CreateDirectory(folderPath);

            // Create the script if it doesn't exist
            string scriptPath = $"Assets/ViewsCreator/{scriptName}.cs";
            if (!File.Exists(scriptPath))
            {
                CreateScriptFile(scriptPath, scriptName);
                AssetDatabase.Refresh();
            }

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

            AttachScriptToGameObject(scriptName, scriptType);
        }

        private static void AttachScriptToGameObject(string scriptName, System.Type scriptType)
        {
            var viewContainer = FindObjectOfType<ViewContainer>();
            GameObject view = CreateRootView(scriptName, scriptType, viewContainer);
           if(view != null)
            {
                if (isCreateShield)
                {
                    CreateShield(view.transform);
                }
                CreateContainer(view);
                Selection.activeGameObject = view;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Logg.Log($"Created GameObject '{scriptName}' with script '{scriptName}' attached.");
        }

        private static GameObject CreateRootView(string scriptName, System.Type scriptType, ViewContainer viewContainer)
        {
            if (string.IsNullOrEmpty(scriptName))
            {
                scriptName = string.Empty;
            }
            var view = new GameObject(scriptName);
            view.layer = LayerMask.NameToLayer("UI");

            //add component require
            view.GetOrAdd<RectTransform>();
            view.GetOrAdd<CanvasRenderer>();
            view.AddComponent(scriptType);

            // set parent
            view.transform.parent = viewContainer != null ? viewContainer.transform : null;

            // set position and scale
            var rectTransform = view.transform.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            return view;
        }

        private static void CreateContainer(GameObject view)
        {
            // create container child
            var container = new GameObject("Container");
            container.layer = LayerMask.NameToLayer("UI");
            container.transform.SetParent(view.transform);
            container.transform.localPosition = Vector3.zero;
            container.transform.localScale = Vector3.one;

            var containerRect = container.GetOrAdd<RectTransform>();
            containerRect.sizeDelta = Vector2.zero;
            containerRect.anchorMin = Vector2.zero;
            containerRect.anchorMax = Vector2.one;
            containerRect.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void CreateShield(Transform parent)
        {
            var shield = new GameObject("Shield");
            shield.layer = LayerMask.NameToLayer("UI");

            Image image = shield.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.9f);

            Transform t = shield.transform;
            t.SetParent(parent.transform);
            t.SetSiblingIndex(0);
            t.localScale = Vector3.one;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0);

            RectTransform rt = t.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMax = new Vector2(2, 2);
            rt.offsetMin = new Vector2(-2, -2);
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