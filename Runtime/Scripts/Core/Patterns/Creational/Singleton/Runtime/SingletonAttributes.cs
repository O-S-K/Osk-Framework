using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GlobalSingletonAttribute : Attribute
    {
        // This attribute is used to mark classes that should be treated as global singletons.
        public bool AutoInitialize { get; }
        public bool IsDontDestroyOnLoad { get; } = true; // Default behavior is to not destroy on load

        public GlobalSingletonAttribute(bool autoInitialize = false, bool isDontDestroyOnLoad = true)
        {
            AutoInitialize = autoInitialize;
            IsDontDestroyOnLoad = isDontDestroyOnLoad;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SceneSingletonAttribute : Attribute
    {
        // This attribute is used to mark classes that should be treated as singletons within a specific scene.
        public string[] AllowedScenes { get; }
        public bool AutoInitialize { get; }
        public bool IsDontDestroyOnLoad { get; } = false; // Default behavior is to not destroy on load

        public SceneSingletonAttribute(bool autoInitialize = false, bool isDontDestroyOnLoad = false,
            params string[] allowedScenes)
        {
            AutoInitialize = autoInitialize;
            AllowedScenes = allowedScenes;
            IsDontDestroyOnLoad = isDontDestroyOnLoad;
        }
    }
}