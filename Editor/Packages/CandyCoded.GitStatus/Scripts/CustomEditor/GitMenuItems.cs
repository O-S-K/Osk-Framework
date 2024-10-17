// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;

namespace CandyCoded.GitStatus
{

    public static class GitMenuItems
    {

        public const int PRIORITY = 5000;

        private static string GetSelectedPath()
        {

            return Path.Combine(Environment.CurrentDirectory, AssetDatabase.GetAssetPath(Selection.activeObject));

        }

        [MenuItem("Git/Discard Changes", false, PRIORITY)]
        [MenuItem("Assets/Discard Changes", false, PRIORITY)]
        private static async void DiscardChanges()
        {

            try
            {

                await Git.DiscardChanges(GetSelectedPath());

            }
            catch (Exception error)
            {

                EditorUtility.DisplayDialog("Error", error.Message, "Ok");

            }

        }

        [MenuItem("Git/Discard Changes", true)]
        [MenuItem("Assets/Discard Changes", true)]
        private static bool ValidateDiscardChanges()
        {

            return Selection.activeObject && File.Exists(GetSelectedPath());

        }

        [MenuItem("Git/Discard All Changes", false, PRIORITY)]
        [MenuItem("Assets/Discard All Changes", false, PRIORITY)]
        private static async void DiscardAllChanges()
        {

            if (EditorUtility.DisplayDialog(
                "Discard all changes",
                $"Are you sure you want to discard all changes in {Selection.activeObject.name}?",
                "Yes",
                "Cancel"))
            {

                try
                {

                    await Git.DiscardChanges(GetSelectedPath());

                }
                catch (Exception error)
                {

                    EditorUtility.DisplayDialog("Error", error.Message, "Ok");

                }

            }

        }

        [MenuItem("Git/Discard All Changes", true)]
        [MenuItem("Assets/Discard All Changes", true)]
        private static bool ValidateDiscardAllChanges()
        {

            return Selection.activeObject && Directory.Exists(GetSelectedPath());

        }

        [MenuItem("Git/Reset Settings", false, PRIORITY + 100)]
        public static void ResetSettings()
        {

            if (EditorUtility.DisplayDialog(
                "Reset package settings",
                $"Are you sure you want to reset all package settings?",
                "Yes",
                "Cancel"))
            {

                GitSettings.Reset();

            }

        }

    }

}

#endif
