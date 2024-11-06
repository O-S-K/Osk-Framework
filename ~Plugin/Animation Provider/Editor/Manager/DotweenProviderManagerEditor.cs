using DG.DOTweenEditor;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(DotweenProviderManager))]
    class DotweenProviderManagerEditor : Editor
    {
        DotweenProviderManager manager;
        private HideFlags cached;
        const string info = @"Used to drive itself and its child nodes to mount Provider";
        float timePreview   = 0;
        
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
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode && manager.gameObject.activeInHierarchy && (manager.hideFlags != HideFlags.NotEditable || manager.IsPreviewing());
            EditorStyles.helpBox.fontSize = 13;
            EditorGUILayout.HelpBox(info, MessageType.Info);
            bool isPreviewing = manager.IsPreviewing();
            
            
            if (GUILayout.Button(isPreviewing ? "Stop preview" : "Start preview"))            
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
        }
    }
}