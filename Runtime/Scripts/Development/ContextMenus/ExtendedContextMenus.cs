#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    public class ExtendedContextMenus
    {
        private static List<Component> copiedComponents = new List<Component>();

        // ---------------- COPY COMPONENTS ----------------
        [MenuItem("CONTEXT/Transform/📋 Copy All Components")]
        private static void CopyAllComponents(MenuCommand command)
        {
            Transform targetTransform = (Transform)command.context;
            copiedComponents.Clear(); // Xóa danh sách cũ

            copiedComponents.AddRange(targetTransform.GetComponents<Component>());

            Debug.Log($"Copied {copiedComponents.Count} components from '{targetTransform.gameObject.name}'");
        }

        // ---------------- PASTE COMPONENTS ----------------
        [MenuItem("CONTEXT/Transform/📄 Paste or Replace Components")]
        private static void PasteOrReplaceComponents(MenuCommand command)
        {
            Transform targetTransform = (Transform)command.context;
            GameObject targetGameObject = targetTransform.gameObject;

            if (copiedComponents.Count == 0)
            {
                Debug.LogWarning("No components copied!");
                return;
            }

            foreach (Component original in copiedComponents)
            {
                if (original is Transform) continue; // Bỏ qua Transform

                // Kiểm tra xem component đã tồn tại chưa
                Component existingComponent = targetGameObject.GetComponent(original.GetType());

                if (existingComponent != null)
                {
                    // Nếu đã có component => Replace thông số
                    EditorUtility.CopySerialized(original, existingComponent);
                    Debug.Log($"Updated existing {original.GetType().Name} on '{targetGameObject.name}'");
                }
                else
                {
                    // Nếu chưa có component => Add mới
                    Component newComponent = targetGameObject.AddComponent(original.GetType());
                    EditorUtility.CopySerialized(original, newComponent);
                    Debug.Log($"Added new {original.GetType().Name} to '{targetGameObject.name}'");
                }
            }
        }
    }
}

#endif