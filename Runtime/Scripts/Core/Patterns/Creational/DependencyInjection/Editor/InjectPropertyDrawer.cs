#if UNITY_EDITOR
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

            var isInterface = fieldInfo.FieldType.IsInterface;
            var oldColor = GUI.color;
 
            // Use Odin's Show Inspector to display the icon in the inspector
            if (isInterface)
            { 
                GUI.color = fieldInfo.FieldType.IsInterface ? Color.green : Color.red;
            }
            else
            { 
                GUI.color = property.objectReferenceValue != null ? Color.green : Color.red;
            }
 
            GUI.DrawTexture(iconRect, LoadIcon());
            GUI.color = oldColor; 
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif