using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomPropertyDrawer(typeof(InjectAttribute))]
    public class InjectPropertyDrawer : PropertyDrawer
    {
        private Texture2D icon;

        private Texture2D LoadIcon()
        {
            if (icon == null) icon = Resources.Load<Texture2D>("Sprites/InjectIcon");
            return icon;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var iconRect = new Rect(position.x, position.y, 20, 20);
            position.xMin += 24;

            // Check if the field is null or an interface and handle accordingly.
            if (fieldInfo.FieldType.IsInterface)
            {
                var oldColor = GUI.color;
                var c = fieldInfo.FieldType.IsInterface ? Color.green : Color.red;
                GUI.color = c;
                GUI.DrawTexture(iconRect, LoadIcon());
                GUI.color = oldColor;
            }
            else
            {
                var oldColor = GUI.color;
                GUI.color = property.objectReferenceValue == null ? Color.red : Color.green;
                GUI.DrawTexture(iconRect, LoadIcon());
                GUI.color = oldColor;
            }
            EditorGUI.PropertyField(position, property, label);
        }
    }
}