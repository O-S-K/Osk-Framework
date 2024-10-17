// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CandyCoded.GitStatus
{

    public static class GitSettings
    {

        private const string EDITOR_PREFS_SCOPE = "xyz.candycoded.gitstatus";

        public static string KEY_GITPATH => $"{EDITOR_PREFS_SCOPE}_gitpath";

        public static string DefaultGitPath => SystemInfo.operatingSystemFamily.Equals(OperatingSystemFamily.Windows)
            ? @"C:\Program Files\Git\bin\git.exe"
            : "/usr/bin/git";

        public static string GitPath
        {
            get => EditorPrefs.GetString(KEY_GITPATH, DefaultGitPath);
            set => EditorPrefs.SetString(KEY_GITPATH, value);
        }

        public static void Reset()
        {

            EditorPrefs.DeleteKey(KEY_GITPATH);

        }

    }

    public class GitSettingsProvider : SettingsProvider
    {

        public GitSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path,
            scopes,
            keywords)
        {

        }

        public override void OnGUI(string searchContext)
        {

            GitSettings.GitPath = EditorGUILayout.TextField("Git Path", GitSettings.GitPath);

        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {

            return new GitSettingsProvider("CandyCoded/GitStatus", SettingsScope.Project);

        }

    }

}
#endif
