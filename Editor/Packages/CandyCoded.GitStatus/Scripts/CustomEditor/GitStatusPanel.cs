// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CandyCoded.GitStatus
{

    public class GitStatusPanel : EditorWindow
    {

        [MenuItem("Git/Git Status", false, GitMenuItems.PRIORITY - 100)]
        public static void ShowWindow()
        {

            GetWindow(typeof(GitStatusPanel), false, "Git Status", true);

        }

        private void OnGUI()
        {

            if (!GitStatus.isGitRepo)
            {

                if (GUILayout.Button("Initialize git repo"))
                {

                    Task.Run(async () =>
                    {

                        await Git.Init();

                        await GitIgnore.Create(Environment.CurrentDirectory);

                        GitStatus.Update();

                    });

                }

                return;

            }

            GUILayout.Space(5);

            var selectedBranch = Array.IndexOf(GitStatus.branches, GitStatus.branch);

            if (selectedBranch == -1)
            {

                GUILayout.Label($"Branch: {GitStatus.branch}");

            }
            else
            {

                selectedBranch = EditorGUILayout.Popup("Branch:", selectedBranch, GitStatus.branches);

                if (!GitStatus.branches[selectedBranch].Equals(GitStatus.branch))
                {

                    if (GitStatus.changedFiles?.Length > 0)
                    {

                        EditorUtility.DisplayDialog(
                            "Unable to checkout branch",
                            $"Unable to checkout {GitStatus.branches[selectedBranch]} as with {GitStatus.changedFiles?.Length} changes. " +
                            "Commit, discard or stash before checking out a different branch.",
                            "Ok");

                    }
                    else
                    {

                        Task.Run(async () =>
                        {

                            await Git.CheckoutBranch(GitStatus.branches[selectedBranch]);

                        });

                        EditorApplication.ExecuteMenuItem("Assets/Refresh");

                    }

                }

            }

            GUILayout.Label($"Number of Changes: {GitStatus.changedFiles?.Length}");
            GUILayout.Label($"Untracked Files: {GitStatus.untrackedFiles?.Length}");
            GUILayout.Label($"Last Updated: {GitStatus.lastUpdated}");

            if (GUILayout.Button("Refresh"))
            {

                GitStatus.Update();

            }

        }

        private void Update()
        {

            if (EditorApplication.isPlaying || EditorApplication.isPaused)
            {

                return;

            }

            Repaint();

        }

    }

}

#endif
