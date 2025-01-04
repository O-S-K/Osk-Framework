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

            // Create the script if it doesn't exist
            string scriptPath = $"Assets/{scriptName}.cs";
            if (!File.Exists(scriptPath))
            {
                CreateScriptFile(scriptPath, scriptName);
                AssetDatabase.Refresh();
            }

            // Wait for Unity to compile the script
            EditorApplication.delayCall += () =>
            {
                string[] guids = AssetDatabase.FindAssets(scriptName + " t:MonoScript");
                if (guids.Length > 0)
                {
                    string scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);

                    if (monoScript == null)
                    {
                        EditorUtility.DisplayDialog("Error",
                            "Failed to create or load the script. Check for compilation errors.", "OK");
                        return;
                    }

                    // Create a new GameObject and attach the script
                    var viewContainer = FindObjectOfType<ViewContainer>();
                    GameObject view = new GameObject(scriptName);
                    view.GetOrAdd<RectTransform>();
                    view.GetOrAdd<CanvasRenderer>();
                    view.AddComponent(monoScript.GetClass());

                    if (viewContainer != null)
                        view.transform.SetParent(viewContainer.transform);

                    view.transform.localPosition = Vector3.zero;
                    view.transform.localScale = Vector3.one;
                    view.transform.Get<RectTransform>().sizeDelta = Vector2.zero;
                    view.transform.Get<RectTransform>().anchorMin = Vector2.zero;
                    view.transform.Get<RectTransform>().anchorMax = Vector2.one;
                    view.transform.Get<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

                    var container = new GameObject("Container");
                    container.transform.SetParent(view.transform);
                    container.transform.localPosition = Vector3.zero;
                    container.transform.localScale = Vector3.one;

                    container.GetOrAdd<RectTransform>();
                    container.transform.Get<RectTransform>().sizeDelta = Vector2.zero;
                    container.transform.Get<RectTransform>().anchorMin = Vector2.zero;
                    container.transform.Get<RectTransform>().anchorMax = Vector2.one;
                    container.transform.Get<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

                    Selection.activeGameObject = view;

                    Debug.Log($"Created GameObject '{scriptName}' with script '{scriptName}' attached.");
                }
                else
                {
                    Debug.LogError($"MonoScript '{scriptName}' not found.");
                }
            };
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