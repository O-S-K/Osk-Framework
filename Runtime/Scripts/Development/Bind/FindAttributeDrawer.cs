using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FindAttribute))]
[CustomPropertyDrawer(typeof(FindChildAttribute))]
[CustomPropertyDrawer(typeof(FindParentAttribute))]
public class FindAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width -= 60; // Chừa chỗ cho nút Update
        EditorGUI.PropertyField(position, property, label);

        position.x += position.width + 5;
        position.width = 55;

        // Hiển thị nút Update
        if (GUI.Button(position, "Update"))
        {
            var mono = property.serializedObject.targetObject as MonoBehaviour;
            if (mono != null)
            {
                // Gọi phương thức AutoAssignComponents để gán các component
                ComponentFinder.AutoAssignComponents(mono);
                // Đánh dấu MonoBehaviour là dirty để Unity biết cần lưu thay đổi
                EditorUtility.SetDirty(mono);
                Debug.Log($"Updated {property.name} in {mono.name}.");
            }
        }
    }
}