#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializeFieldDictionary<,>))]
public class SerializeFieldDictionaryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Lấy keys và values
        SerializedProperty keys = property.FindPropertyRelative("keys");
        SerializedProperty values = property.FindPropertyRelative("values");

        // Kiểm tra keys và values có tồn tại không
        if (keys == null || values == null)
        {
            EditorGUI.LabelField(position, label.text, "Missing keys or values is object or type.");
            EditorGUI.EndProperty();    
            return;
        }
          

        // Tính chiều cao
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float totalHeight = lineHeight * (keys.arraySize + 1);
        Rect rect = EditorGUI.PrefixLabel(position, label);

        // Hiển thị kích thước của dictionary
        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), "Dictionary Size: " + keys.arraySize);
        EditorGUI.LabelField(new Rect(rect.x, rect.y + lineHeight, rect.width, lineHeight), "Keys and Values:");

        // Hiển thị keys và values
        for (int i = 0; i < keys.arraySize; i++)
        {
            Rect keyRect = new Rect(rect.x, rect.y + (i + 1) * lineHeight, rect.width / 2 - 5, lineHeight);
            Rect valueRect = new Rect(rect.x + (rect.width / 2 + 5), rect.y + (i + 1) * lineHeight, rect.width / 2 - 5, lineHeight);

            // Lấy key và value
            SerializedProperty keyProperty = keys.GetArrayElementAtIndex(i);
            SerializedProperty valueProperty = values.GetArrayElementAtIndex(i);

            // Hiển thị key
            EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none);

            // Kiểm tra xem value có phải là kiểu Object hay không
            if (valueProperty.propertyType == SerializedPropertyType.ObjectReference)
            {
                // Nếu là ObjectReference, hiển thị giá trị của ObjectReference
                EditorGUI.ObjectField(valueRect, valueProperty, GUIContent.none);
            }
            // else if (valueProperty.propertyType == SerializedPropertyType.Boolean)
            // {
            //     // Nếu là ObjectReference, hiển thị giá trị của ObjectReference
            //     EditorGUI.Toggle(valueRect, valueProperty.boolValue ? "True" : "False", valueProperty.boolValue);
            // }
            else
            {
                // Nếu không phải ObjectReference, hiển thị giá trị nguyên bản
                EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            }
        }

        // Hiển thị nút thêm hoặc xóa tùy thuộc vào số lượng mục
        if (keys.arraySize == 0)
        {
            if (GUI.Button(new Rect(rect.x, rect.y + totalHeight, rect.width, lineHeight), "Add Entry"))
            {
                keys.InsertArrayElementAtIndex(keys.arraySize);
                values.InsertArrayElementAtIndex(values.arraySize);
            }
        }
        else
        {
            // Nếu có ít nhất một phần tử, hiển thị nút xóa
            if (GUI.Button(new Rect(rect.x, rect.y + totalHeight, rect.width, lineHeight), "Remove Last Entry"))
            {
                keys.DeleteArrayElementAtIndex(keys.arraySize - 1);
                values.DeleteArrayElementAtIndex(values.arraySize - 1);
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty keys = property.FindPropertyRelative("keys");

        // Kiểm tra keys có tồn tại không
        if (keys == null)
        {
            return EditorGUIUtility.singleLineHeight; // Trả về chiều cao mặc định nếu keys bị thiếu
        }

        float lineHeight = EditorGUIUtility.singleLineHeight;
        return lineHeight * (keys.arraySize + 5); // +1 cho nhãn kích thước, +1 cho nút thêm/xóa
    }
}
#endif
