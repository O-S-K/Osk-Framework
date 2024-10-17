// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CandyCoded.GitStatus
{

    [InitializeOnLoad]
    public static class FileWatcher
    {

        public delegate void EventHandler();

        public static event EventHandler UpdateEvent;

        private static readonly FileSystemWatcher _fileSystemWatcher;

        private static bool _changeRegistered;

        static FileWatcher()
        {

            if (_fileSystemWatcher != null)
            {

                return;

            }

            _fileSystemWatcher = new FileSystemWatcher(Application.dataPath);

            _fileSystemWatcher.Created += OnChanged;
            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Deleted += OnChanged;

            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Filter = "*.*";
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.EnableRaisingEvents = true;

        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {

            if (e.FullPath.EndsWith(".meta"))
            {

                return;

            }

            if (!_changeRegistered)
            {

                EditorApplication.update += OnAllChanged;

                _changeRegistered = true;

            }

        }

        private static void OnAllChanged()
        {

            UpdateEvent?.Invoke();

            EditorApplication.update -= OnAllChanged;

            _changeRegistered = false;

        }

    }

}
#endif
