using UnityEngine;
using UnityEditor;
using Plugins;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Editors
{
    public class ReplaceFontEditorWindow : EditorWindow
    {
        private static float sizeX = 350;
        private static float sizeY = 95;
        private static EditorWindow window;


        private Font newFont;
        private TMP_FontAsset newTMPfont;
        [SerializeField] private List<UnityEngine.UI.Text> specifiedLegacyTextObjects = new();
        [SerializeField] private List<TextMeshProUGUI> specifiedTMPObjects = new();
        private WindowType windowType;
        private FontType fontType;
        private Vector2 scrollPosition = Vector2.zero;

        public static void ShowWindow()
        {
            window = GetWindow(typeof(ReplaceFontEditorWindow));
            window.titleContent = new GUIContent("Replace font Tool");

            sizeY = 95;
            sizeX = 350;
            window.maxSize = new Vector2(sizeX, sizeY);
            window.minSize = new Vector2(sizeX, sizeY);
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, sizeY == 500, GUILayout.Width(window.maxSize.x),
            GUILayout.Height(window.maxSize.y));

            GUILayout.Space(10);

            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty specifiedLegacyTextObjectsProperty = so.FindProperty(nameof(specifiedLegacyTextObjects));
            SerializedProperty specifiedTMPObjectsProperty = so.FindProperty(nameof(specifiedTMPObjects));

            if (fontType == FontType.LegacyText)
                newFont = EditorGUILayout.ObjectField("New font", newFont, typeof(Font), false) as Font;
            else newTMPfont = EditorGUILayout.ObjectField("New font", newTMPfont, typeof(TMP_FontAsset), false) as TMP_FontAsset;

            if (windowType == WindowType.Specified)
            {
                EditorGUILayout
                .PropertyField(fontType == FontType.LegacyText ? specifiedLegacyTextObjectsProperty : specifiedTMPObjectsProperty, true);
                sizeY = 95 +
                EditorGUI.GetPropertyHeight(fontType == FontType.LegacyText ?
                specifiedLegacyTextObjectsProperty : specifiedTMPObjectsProperty, true)
                + (fontType == FontType.LegacyText ? (specifiedLegacyTextObjects.Count != 0 ? 20 : 0)
                : (specifiedTMPObjects.Count != 0 ? 20 : 0));
                sizeY = Mathf.Clamp(sizeY, 90, 500);
            }
            else sizeY = 95;

            windowType = (WindowType)EditorGUILayout.EnumPopup("Find place:", windowType);

            fontType = (FontType)EditorGUILayout.EnumPopup("Type:", fontType);

            if (GUILayout.Button("Replace"))
            {
                switch (windowType)
                {
                    case WindowType.CurrentScene:
                        if (fontType == FontType.LegacyText) ReplaceFont.ReplaceFontInScene(newFont);
                        else ReplaceFont.ReplaceFontInScene(newTMPfont);
                        break;

                    case WindowType.Prefabs:
                        if (fontType == FontType.LegacyText) ReplaceFont.ReplaceFontPrefab(newFont);
                        else ReplaceFont.ReplaceFontPrefab(newTMPfont);
                        break;

                    case WindowType.InProject:
                        if (fontType == FontType.LegacyText) ReplaceFont.ReplaceFontInProject(newFont);
                        else ReplaceFont.ReplaceFontInProject(newTMPfont);
                        break;

                    case WindowType.Specified:
                        if (fontType == FontType.LegacyText) ReplaceFont.ReplaceFontSpecified(newFont, specifiedLegacyTextObjects);
                        else ReplaceFont.ReplaceFontSpecified(newTMPfont, specifiedTMPObjects);
                        break;
                }
            }

            if (windowType == WindowType.Specified)
            {
                bool listIsNotEmpty = fontType == FontType.LegacyText ? specifiedLegacyTextObjects.Count != 0 : specifiedTMPObjects.Count != 0;
                if (listIsNotEmpty)
                    if (GUILayout.Button("Clear"))
                    {
                        if (fontType == FontType.LegacyText) specifiedLegacyTextObjects.Clear();
                        else specifiedTMPObjects.Clear();
                    }
            }

            GUILayout.EndScrollView();
            so.ApplyModifiedProperties();

            if (window != null)
            {
                if (sizeY == 500) sizeX = 350 + 13;
                else sizeX = 350;

                window.maxSize = new Vector2(sizeX, sizeY);
                window.minSize = new Vector2(sizeX, sizeY);
            }
        }
    }

    public enum WindowType
    { [InspectorName("Current scene")] CurrentScene, Prefabs, Specified, [InspectorName("All files in project")] InProject }

    public enum FontType
    { LegacyText, TextMeshPro }
}