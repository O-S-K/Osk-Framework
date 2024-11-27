using DG.DOTweenEditor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(DotweenProviderManager))]
    class DotweenProviderManagerEditor : OdinEditor
    {
        DotweenProviderManager manager;
        private HideFlags cached;
        const string info = @"Used to drive itself and its child nodes to mount Provider";

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            if (DOTweenEditorPreview.isPreviewing)
            {
                manager.StopPreview();
            }
        }

        private void OnEnable()
        {
            manager = target as DotweenProviderManager;
            //manager.gameObject.hideFlags = HideFlags.None;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        //Restore the Dotween animation preview when the user presses the Play button
        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
            {
                manager.StopPreview();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode && manager.gameObject.activeInHierarchy &&
                          (manager.hideFlags != HideFlags.NotEditable || manager.IsPreviewing());
            EditorStyles.helpBox.fontSize = 12;
            EditorGUILayout.HelpBox(info, MessageType.Info);
            bool isPreviewing = manager.IsPreviewing();

            GUIStyle buttons = new GUIStyle(GUI.skin.button);
            buttons.normal.background =
                MakeColorTexture(!isPreviewing ? new Color32(46, 139, 87, 255) : new Color32(178, 11, 11, 255));
 
            string buttonText = $"{(isPreviewing ? "Stop preview" : "Start preview")}";

            if (GUILayout.Button(buttonText, buttons))
            {
                if (isPreviewing)
                {
                    manager.StopPreview();
                }
                else
                {
                    manager.StartPreview();
                }
            }

            buttons.normal.background = MakeColorTexture(Color.grey);

            if (GUILayout.Button("Get Providers", buttons))
            {
                manager.AddProvider();
            }
        }

        private Texture2D MakeColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}