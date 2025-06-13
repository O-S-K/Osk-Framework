#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace OSK
{
    [CustomEditor(typeof(BaseReferenceHolder), true)]
    public class EditorReferenceHolder : Editor
    {
        private BaseReferenceHolder m_RefHolder;
        private ReorderableList m_ReorderLst;
        [SerializeField] 
        private string searchName = "";
        private HashSet<RefData> m_SearchMatchedData = new HashSet<RefData>();
        private string textCodeGeneration = "";
        private Vector2 scrollPosition;

        protected virtual void OnEnable()
        {
            m_RefHolder = this.target as BaseReferenceHolder;
            this.m_ReorderLst = new ReorderableList(this.serializedObject, this.serializedObject.FindProperty("DataRefs"));
            this.m_ReorderLst.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawChild);
            this.m_ReorderLst.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddButton);
            this.m_ReorderLst.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawHeader);
            this.m_ReorderLst.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveButton);
        }

        public override void OnInspectorGUI()
        {             
            EditorGUILayout.BeginHorizontal();
            this.ShowSearchTool();
            EditorGUILayout.EndHorizontal();
            this.m_ReorderLst.DoLayoutList();
            GUI.color = Color.white;
            this.ShowAutoImport();
            this.ShowSaveAll();
            EditorGUILayout.BeginHorizontal();
            this.ShowClearAll();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Gen Code", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.BeginHorizontal();
            this.GenerateCode();
            CopyCodeGeneration();
            ClearCodeGeneration();

            this.OnShowInspectorGUI();
            EditorGUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
            textCodeGeneration = GUILayout.TextField(textCodeGeneration, GUILayout.Height(100));
            GUILayout.EndScrollView();
            
            base.OnInspectorGUI();
        }

        protected virtual void OnShowInspectorGUI()
        {
        }

        private void GenerateCode()
        {
            if (!GUILayout.Button("Generate Code"))
                return;
            // Get the active object, it could be a prefab or scene instance
            var selectedObject = Selection.activeObject;

            // Check if the selected object is a prefab instance
            GameObject go = PrefabUtility.GetPrefabInstanceHandle(selectedObject) as GameObject;
            if (go == null)
            {
                go = selectedObject as GameObject; // Use the selected object as is if it's not a prefab instance
            }

            if (go == null)
            {
                Debug.LogError("Please select a GameObject or a prefab instance.");
                return;
            }

            // Get the BaseReferenceHolder component on the selected object or prefab instance
            var refHolder = go.GetComponent<BaseReferenceHolder>();
            if (refHolder == null)
            {
                Debug.LogError("BaseReferenceHolder not found on the selected object.");
                return;
            }

            StringBuilder codeBuilder = new StringBuilder();

            foreach (var data in refHolder.DataRefs)
            {
//                Debug.Log("Data: " + data.name + ", Type: " + data.TypeName);
                codeBuilder.AppendLine(
                    $"public {data.TypeName} {FormatName(data.name, data.TypeName)} => GetRef<{data.TypeName}>(\"{data.name}\");");
            }

            string generatedCode = codeBuilder.ToString();
            Debug.Log(generatedCode);
            textCodeGeneration = generatedCode;
        }

        private string FormatName(string input, string typeComponent, bool isLower = true)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Remove unwanted characters, keeping only word characters (letters, digits, underscores)
            input = Regex.Replace(input, @"[^\w]", string.Empty);
            string suffix = GetSuffixFromType(typeComponent);

            // Remove the suffix if it's already part of the input (case-insensitive)
            if (!string.IsNullOrEmpty(suffix) &&
                input.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                input = input.Substring(0, input.Length - suffix.Length);
            }
            
            // Ensure the first character is lowercase
            if (isLower && input.Length > 0)
            {
                input = char.ToLower(input[0]) + input.Substring(1);
            }

            // Check if the input already ends with the type component (e.g., "Button", "Text")
            if (!input.EndsWith(typeComponent, StringComparison.OrdinalIgnoreCase))
            {
                // Append the type component if it doesn't exist already
                input += NameType((EComponentType)Enum.Parse(typeof(EComponentType), typeComponent));
            }

            return input;
        } 
        private string NameType(EComponentType type)
        {
            switch (type)
            {
                case EComponentType.ScrollRect:
                    return "ScrollRect";
                case EComponentType.Button:
                    return "Button";
                case EComponentType.Image:
                    return "Image";
                case EComponentType.Text:
                    return "Text";
                case EComponentType.TextMeshProUGUI:
                    return "Text";
                case EComponentType.Tab:
                    return "Tab";
                case EComponentType.Toggle:
                    return "Toggle";
                case EComponentType.Dropdown:
                    return "Dropdown";
                case EComponentType.InputField:
                    return "InputField";
                case EComponentType.LayoutGroup:
                    return "LayoutGroup";
                case EComponentType.RawImage:
                    return "RawImage";
                case EComponentType.Camera:
                    return "Camera";
                case EComponentType.CanvasGroup:
                    return "CanvasGroup";
                case EComponentType.Slider:
                    return "Slider";
                case EComponentType.RectTransform:
                    return "Rect";
                case EComponentType.Transform:
                    return "Transform";
                case EComponentType.Animator:
                    return "Animator";
                case EComponentType.MonoBehaviour:
                    return "";
                case EComponentType.GameObject:
                    return "GameObject";
                default: 
                    return nameof(EComponentType);
            }
        }
        
        private string GetSuffixFromType(string typeName)
        {
            switch (typeName)
            {
                case "Button":
                    return "Button";
                case "TextMeshProUGUI":
                case "TextMeshPro":
                case "Text":
                case "TextMesh":
                    return "Text";
                case "Image":
                    return "Image";
                case "Slider":
                    return "Slider";
                case "Toggle":
                    return "Toggle";
                case "InputField":
                case "TMP_InputField":
                    return "Input";
                case "Dropdown":
                case "TMP_Dropdown":
                    return "Dropdown";
                case "RawImage":
                    return "RawImage";
                case "RectTransform":
                    return "Rect";
                case "Transform":
                    return "Transform";
                case "GameObject":
                    return ""; // No suffix
                default:
                    return ""; // Unknown type, no suffix
            }
        }

        private void CopyCodeGeneration()
        {
            if (!GUILayout.Button("Copy Code"))
                return;

            if (string.IsNullOrEmpty(textCodeGeneration))
            {
                Debug.LogError("No code to copy. Please generate code first.");
                return;
            }

            TextEditor textEditor = new TextEditor
            {
                text = textCodeGeneration
            };
            textEditor.SelectAll();
            textEditor.Copy();
        }

        private void ClearCodeGeneration()
        {
            if (!GUILayout.Button("Clear Code"))
                return;

            textCodeGeneration = string.Empty;
        }


        public Rect[] GetRects(Rect r)
        {
            Rect[] rects = new Rect[6];
            float width1 = r.width;
            float width2 = 50f;
            float width3 = 50f;
            float width4 = 40f;
            float num1 = 4f;
            float width5 =
                (float)Mathf.FloorToInt((float)(((double)width1 - (double)width2 - (double)width3 - (double)width4 -
                                                 (double)num1 * 5.0) / 3.0));
            int num2 = 0;
            float x1 = r.x;
            Rect[] rectArray1 = rects;
            int index1 = num2;
            int num3 = index1 + 1;
            Rect rect1 = new Rect(x1, r.y, width2, r.height);
            rectArray1[index1] = rect1;
            float x2 = x1 + (width2 + num1);
            Rect[] rectArray2 = rects;
            int index2 = num3;
            int num4 = index2 + 1;
            Rect rect2 = new Rect(x2, r.y, width5, r.height);
            rectArray2[index2] = rect2;
            float x3 = x2 + (width5 + num1);
            Rect[] rectArray3 = rects;
            int index3 = num4;
            int num5 = index3 + 1;
            Rect rect3 = new Rect(x3, r.y, width3, r.height);
            rectArray3[index3] = rect3;
            float x4 = x3 + (width3 + num1);
            Rect[] rectArray4 = rects;
            int index4 = num5;
            int num6 = index4 + 1;
            Rect rect4 = new Rect(x4, r.y, width5, r.height);
            rectArray4[index4] = rect4;
            float x5 = x4 + (width5 + num1);
            Rect[] rectArray5 = rects;
            int index5 = num6;
            int num7 = index5 + 1;
            Rect rect5 = new Rect(x5, r.y, width5, r.height);
            rectArray5[index5] = rect5;
            float x6 = x5 + (width5 + num1);
            Rect[] rectArray6 = rects;
            int index6 = num7;
            int num8 = index6 + 1;
            Rect rect6 = new Rect(x6, r.y, width4, r.height);
            rectArray6[index6] = rect6;
            return rects;
        }

        private void DrawChild(Rect r, int index, bool selected, bool focused)
        {
            if (index >= m_ReorderLst.serializedProperty.arraySize)
                return;
            RefData data = this.m_RefHolder.DataRefs[index];
            if (string.IsNullOrEmpty(data.TypeName))
                return;
            m_ReorderLst.serializedProperty.GetArrayElementAtIndex(index);
            ++r.y;
            r.height = 16f;
            Rect[] rects = this.GetRects(r);
            GUI.color = Color.white;
          
            
            bool isDuplicate = CheckDuplicateName(data.name, index);
            if (isDuplicate)
            {
                GUI.color = new Color(1f, 0.5f, 0.5f); // Color for duplicate names
            }
            else
            {
                GUI.color = Color.white; // Default color for non-duplicates
            }
            if (m_SearchMatchedData.Contains(data))
                GUI.color = Color.yellow;
            
            
            if (!GUI.Button(rects[0], index.ToString()));
                
            rects[1].width += 10;
            data.name = EditorGUI.TextField(rects[1],  data.name);
            if (GUI.Button(rects[2], "→") && (Object)data.GetGameObject() != (Object)null)
            {
                data.GetGameObject().name = FormatName(data.name, data.TypeName, false);
                this.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(data.GetGameObject());
            }

            EComponentType componentType = (EComponentType)Enum.Parse(typeof(EComponentType), data.TypeName);
            if (componentType <= EComponentType.GameObject)
            {
                Object @object = EditorGUI.ObjectField(rects[3], data.bindObj, typeof(Object), true);
                if (data.bindObj != @object)
                {
                    data.bindObj = @object;
                    data = this.GetData(data.GetGameObject());
                    this.serializedObject.ApplyModifiedProperties();
                }
            }

            if (!string.IsNullOrEmpty(data.TypeName))
            {
                string str = EditorGUI
                    .EnumPopup(rects[4], (EComponentType)Enum.Parse(typeof(EComponentType), data.TypeName)).ToString();
                if (data.TypeName != str)
                {
                    data.TypeName = str;
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUI.Button(rects[5], "×"))
            {
                UnityEditor.EditorUtility.SetDirty(m_RefHolder.gameObject);
                m_ReorderLst.serializedProperty.DeleteArrayElementAtIndex(index);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private bool CheckDuplicateName(string name, int currentIndex)
        {
            // Check if there's any other entry with the same name
            for (int index = 0; index < this.m_RefHolder.DataRefs.Count; ++index)
            {
                if (index != currentIndex && this.m_RefHolder.DataRefs[index].name == name)
                {
                    return true; // Duplicate found
                }
            }
            return false; // No duplicates
        }

        private void DrawHeader(Rect headerRect)
        {
            headerRect.xMin += 14f;
            ++headerRect.y;
            headerRect.height = 15f;
            Rect[] rects = this.GetRects(headerRect);
            int index1 = 0;
            string[] strArray = new string[6]
            {
                "Order",
                "Name",
                "Rename",
                "Content",
                "Type",
                "Delete"
            };
            for (int index2 = 0; index2 < rects.Length; ++index2)
            {
                GUI.Label(rects[index1], strArray[index2], EditorStyles.label);
                ++index1;
            }
        }

        private void RemoveButton(ReorderableList list)
        {
            this.m_ReorderLst.serializedProperty.DeleteArrayElementAtIndex(list.index);
            this.serializedObject.ApplyModifiedProperties();
        }

        public void AddButton(ReorderableList list)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add Component Param"), false,
                new GenericMenu.MenuFunction(this.AddComponentData));
            genericMenu.ShowAsContext();
        }

        public void AddComponentData()
        {
            this.m_RefHolder.DataRefs.Add(new RefData("New GameObject", EComponentType.GameObject));
            this.serializedObject.ApplyModifiedProperties();
        }


        private void ShowSearchTool()
        {
            GUILayout.Label("Search", EditorStyles.boldLabel, GUILayout.Width(50f));
            this.searchName = EditorGUILayout.TextField(this.searchName);
            if (GUILayout.Button("×", GUILayout.Width(50f)))
                this.searchName = string.Empty;
            this.SearchDatas(this.searchName);
        }

        public void SearchDatas(string searchName)
        {
            this.m_SearchMatchedData.Clear();
            if (string.IsNullOrEmpty(searchName))
                return;
            for (int index = 0; index < this.m_RefHolder.DataRefs.Count; ++index)
            {
                if (this.m_RefHolder.DataRefs[index].name.Contains(searchName))
                    this.m_SearchMatchedData.Add(this.m_RefHolder.DataRefs[index]);
            }
        }

        private void ShowAutoImport()
        {
            GUILayout.Label("Drag object to here",  EditorStyles.boldLabel,GUILayout.Width(200f));
            GameObject go = EditorGUILayout.ObjectField((Object)null, typeof(GameObject), true, new GUILayoutOption[1]
            {
                GUILayout.Height(50f)
            }) as GameObject;
            if (!((Object)go != (Object)null))
                return;
            if (go.transform.childCount > 0 && EditorUtility.DisplayDialog("Warning:",
                    "Should import children nodes!\r\n\r\nCurrent node\n-" + go.name, "Yes", "No"))
            {
                foreach (Transform componentsInChild in go.GetComponentsInChildren<Transform>(true))
                {
                    if ((Object)componentsInChild != (Object)go.transform)
                    {
                        this.AddGo(componentsInChild.gameObject);
                    }
                }
            }

            this.AddGo(go);
            UnityEditor.EditorUtility.SetDirty(go);
            this.serializedObject.ApplyModifiedProperties();
        }
        
        protected void ShowSaveAll()
        {
            if (!GUILayout.Button("Save All"))
                return;
            for (int index = 0; index < this.m_RefHolder.DataRefs.Count; ++index)
            {
                RefData data = this.m_RefHolder.DataRefs[index];
                if (data.bindObj != null)
                {
                    data.bindObj.name = FormatName(data.name, data.TypeName, false);
                    this.serializedObject.ApplyModifiedProperties();
                }
            }
            
            EditorUtility.SetDirty(m_RefHolder.gameObject);
            EditorUtility.SetDirty(m_RefHolder); 
        }

        private void ShowAutoRename()
        {
            if (!GUILayout.Button("Auto rename"))
                return;
            RectTransform[] componentsInChildren =
                this.m_RefHolder.gameObject.GetComponentsInChildren<RectTransform>(true);
            int num = 0;
            for (int index = 0; index < componentsInChildren.Length; ++index)
            {
                if ((Object)componentsInChildren[index].GetComponent<Image>() != (Object)null)
                {
                    componentsInChildren[index].gameObject.name = "Image_" + (object)num;
                    ++num;
                }
            }
        }

        private void ShowClearAll()
        {
            if (!GUILayout.Button("ClearAll"))
                return;
            this.m_ReorderLst.serializedProperty.ClearArray();
            this.serializedObject.ApplyModifiedProperties();
        }

        private void ShowAutoDetect()
        {
            if (!GUILayout.Button("Auto Detect"))
                return;
            for (int index1 = 0; index1 < this.m_RefHolder.DataRefs.Count; ++index1)
            {
                for (int index2 = 0; index2 < this.m_RefHolder.DataRefs.Count; ++index2)
                {
                    if (index1 != index2 && this.m_RefHolder.DataRefs[index1].name == this.m_RefHolder.DataRefs[index2].name)
                        this.m_RefHolder.DataRefs[index1].hasVal = true;
                }
            }

            this.m_RefHolder.DataRefs.Sort(new Comparison<RefData>(this.ListSort));
        }

        private void ShowAutoBinding()
        {
            return;
            if (!GUILayout.Button("AutoBind"))
                return;
            Transform transform = m_RefHolder.transform;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> typeList = new List<Type>();
            Type[] array = assemblies.SelectMany(assembly => assembly.GetTypes()).Where(tt =>
            {
                if (typeof(View).IsAssignableFrom(tt) || typeof(View).IsAssignableFrom(tt))
                    return true;
                return typeof(UIBaseItem).IsAssignableFrom(tt) && !tt.IsAbstract;
            }).ToArray();
            typeList.AddRange(array);
            Type type1 = null;
            foreach (Type type2 in typeList)
            {
                if (type2.Name == transform.name)
                {
                    type1 = type2;
                    break;
                }
            }

            if (type1 == null)
            {
                Debug.Log(("No type " + transform.name));
            }
            else
            {
                m_RefHolder.DataRefs.Clear();
                HashSet<Type> targetTypes = new HashSet<Type>()
                {
                    typeof(Button),
                    typeof(Text),
                    typeof(TextMeshProUGUI),
                    typeof(Camera),
                    typeof(Image),
                    typeof(Toggle),
                    typeof(Slider),
                    typeof(InputField),
                    typeof(LayoutGroup),
                    typeof(Dropdown),
                    typeof(GameObject),
                    typeof(RawImage),
                    typeof(Transform),
                };
                foreach (PropertyInfo propertyInfo in type1.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.FlattenHierarchy)
                             .Where(tt => targetTypes.Contains(tt.PropertyType))
                             .ToArray())
                {
                    string name = propertyInfo.Name;
                    Transform child = RecursiveFindChild(transform, name, propertyInfo.PropertyType);
                    if (child != null)
                    {
                        AddGo(child.gameObject, propertyInfo.PropertyType);
                    }
                }

                m_ReorderLst.serializedProperty.ClearArray();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(transform.gameObject);
            }
        }

        private int ListSort(RefData x, RefData y)
        {
            if (x.hasVal && !y.hasVal)
                return 1;
            return !x.hasVal && y.hasVal ? -1 : 0;
        }

        private void Move(int i, int type)
        {
            switch (type)
            {
                case 1:
                    if (i == 0)
                        break;
                    (m_RefHolder.DataRefs[i], m_RefHolder.DataRefs[i - 1]) = (m_RefHolder.DataRefs[i - 1], m_RefHolder.DataRefs[i]);
                    break;
                case 2:
                    if (i != m_RefHolder.DataRefs.Count - 1)
                    {
                        (m_RefHolder.DataRefs[i], m_RefHolder.DataRefs[i + 1]) =
                            (m_RefHolder.DataRefs[i + 1], m_RefHolder.DataRefs[i]);
                    }

                    break;
            }
        }

        private RefData GetData(GameObject go)
        {
            if (go == null)
                return null;
            RefData data = null;
            for (int index = 0; index < 20; ++index)
            {
                EComponentType eComponent = (EComponentType)index;
                if (eComponent != EComponentType.GameObject)
                {
                    Component component = go.GetComponent(eComponent.ToString());
                    if (component != null)
                    {
                        data = new RefData(go.name, eComponent, component);
                        break;
                    }
                }
            }

            return data ?? new RefData(go.name, EComponentType.GameObject, go);
        }

        protected void AddGo(GameObject go)
        {
            RefData data = GetData(go);
            if (data == null)
                return;
            m_RefHolder.DataRefs.Add(data);
        }

        protected RefData AddGo(GameObject go, Type type)
        {
            RefData data = GetData(go);
            if (data != null)
            {
                data.bindObj = !(type == typeof(GameObject))
                    ? (!(type == typeof(Transform)) ? go.GetComponent(type) : (Object)go.transform)
                    : go;
                data.TypeName = type.Name;
                m_RefHolder.DataRefs.Add(data);
            }

            return data;
        }

        public Transform RecursiveFindChild(Transform parent, string name, Type targetType)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(parent);
            while (queue.Count > 0)
            {
                var trans = queue.Dequeue();
                if (trans.name == name)
                {
                    if (targetType == typeof(GameObject)
                        || targetType == typeof(Transform))
                        return trans;
                    if (trans.gameObject.GetComponent(targetType) != null)
                        return trans;
                }

                foreach (Transform childTran in trans)
                {
                    queue.Enqueue(childTran);
                }
            }

            return null;
        }
    }
}
#endif